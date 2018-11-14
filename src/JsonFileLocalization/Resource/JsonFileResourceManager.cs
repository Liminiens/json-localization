using System;
using System.Globalization;
using System.IO;
using System.Text;
using JsonFileLocalization.Caching;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Resource manager based on JSON files
    /// </summary>
    public class JsonFileResourceManager : IJsonFileResourceManager, IDisposable
    {
        private readonly JsonFileLocalizationSettings _settings;
        private readonly ILoggerFactory _loggerFactory;

        private readonly ConcurrentDictionaryCache<string, JsonFileResource> _resourceCache
            = new ConcurrentDictionaryCache<string, JsonFileResource>();
        private readonly ConcurrentDictionaryCache<string, string> _resourcePathCache
            = new ConcurrentDictionaryCache<string, string>();

        private readonly FileSystemWatcher _resourceFileWatcher;
        private readonly ILogger<JsonFileResourceManager> _logger;

        /// <summary>
        /// Creates an instance of resource manager with provided settings
        /// </summary>
        /// <param name="settings">settings for manager</param>
        /// <param name="loggerFactory">logger factory</param>
        public JsonFileResourceManager(
            JsonFileLocalizationSettings settings,
            ILoggerFactory loggerFactory)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<JsonFileResourceManager>();
            _resourceFileWatcher = new FileSystemWatcher(settings.ResourcesPath)
            {
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                IncludeSubdirectories = true,
                Filter = "*.json"
            };
            _resourceFileWatcher.Changed += ResourceFileWatcherOnChanged;
        }

        ~JsonFileResourceManager()
        {
            Dispose();
        }

        private void ResourceFileWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            var fileChanged = fileSystemEventArgs.ChangeType.HasFlag(WatcherChangeTypes.Changed);
            var fileRenamed = fileSystemEventArgs.ChangeType.HasFlag(WatcherChangeTypes.Renamed);
            var fileDeleted = fileSystemEventArgs.ChangeType.HasFlag(WatcherChangeTypes.Deleted);
            if (fileChanged || fileDeleted || fileRenamed)
            {
                var filePath = fileSystemEventArgs.FullPath;
                if (_resourcePathCache.TryGet(filePath, out var key))
                {
                    _resourcePathCache.Invalidate(filePath);
                    _resourceCache.Invalidate(key);
                    _logger.LogInformation("Deleted resource \"{path}\" with cache key \"{key}\" from cache", filePath, key);
                }
            }
        }

        private string GetFileName(string baseName, string location, CultureInfo culture)
        {
            var fileNameBuilder = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(location))
            {
                fileNameBuilder.AppendFormat("{0}.{1}", location, baseName);
            }
            else
            {
                fileNameBuilder.Append(baseName);
            }

            switch (_settings.CultureSuffixStrategy)
            {
                case CultureSuffixStrategy.TwoLetterISO6391:
                    fileNameBuilder.AppendFormat(".{0}", culture.TwoLetterISOLanguageName);
                    break;
                case CultureSuffixStrategy.TwoLetterISO6391AndCountryCode:
                    fileNameBuilder.AppendFormat(".{0}", culture.Name);
                    break;
                default:
                    break;
            }
            fileNameBuilder.Append(".json");
            return fileNameBuilder.ToString();
        }

        private JsonFileResource CreateResource(string cacheKey,
            JObject content, string baseName, string location, string path, CultureInfo culture)
        {
            _resourcePathCache.TryAdd(path, cacheKey);
            return new JsonFileResource(
                content, baseName, location, path, culture,
                _loggerFactory.CreateLogger<JsonFileResource>());
        }

        private JObject LoadFile(string path)
        {
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream))
            {
                var content = streamReader.ReadToEnd();
                return JObject.Parse(content);
            }
        }
        private JsonFileResource Create(string cacheKey, string baseName, string location, CultureInfo culture)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }
            string path = Path.Combine(_settings.ResourcesPath, GetFileName(baseName.Trim(), location.Trim(), culture));
            if (File.Exists(path))
            {
                return CreateResource(cacheKey, LoadFile(path), baseName, location, path, culture);
            }
            //try to find a file without a location in name
            if (String.IsNullOrWhiteSpace(location))
            {
                _logger.LogDebug("Resource file with culture \"{culture}\" not found: \"{path}\"", culture.Name, path);
                return null;
            }
            string noLocationPath = Path.Combine(_settings.ResourcesPath, GetFileName(baseName.Trim(), String.Empty, culture));

            if (!File.Exists(noLocationPath))
            {
                _logger.LogDebug(
                    "Resource file with culture \"{culture.Name}\" not found on paths: \"{path}\" and \"{noLocationPath}\"",
                    culture.Name, path, noLocationPath);
                return null;
            }
            return CreateResource(cacheKey, LoadFile(noLocationPath), baseName, String.Empty, path, culture);
        }

        /// <inheritdoc />
        public JsonFileResource GetResource(string baseName, string location, CultureInfo culture)
        {
            string cacheKey = $"baseName={baseName};location={location};culture={culture.Name};";
            return _resourceCache.GetOrAdd(cacheKey, key => Create(key, baseName, location, culture));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _resourceFileWatcher.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
