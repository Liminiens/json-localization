using JsonFileLocalization.Caching;
using JsonFileLocalization.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Resource manager based on JSON files
    /// </summary>
    public class JsonFileResourceManager : IJsonFileResourceManager, IDisposable
    {
        private readonly ResourceFileManager _fileManager = new ResourceFileManager();

        private readonly JsonFileLocalizationSettings _settings;
        private readonly ILoggerFactory _loggerFactory;

        private readonly ConcurrentDictionaryCache<string, JsonFileResource> _resourceCache
            = new ConcurrentDictionaryCache<string, JsonFileResource>();

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
            if (settings.WatchForChanges)
            {
                _resourceFileWatcher = new FileSystemWatcher(settings.ResourcesPath)
                {
                    EnableRaisingEvents = true,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                    IncludeSubdirectories = true,
                    Filter = "*.json"
                };
                _resourceFileWatcher.Changed += ResourceFileWatcherOnChanged;
            }
        }

        ~JsonFileResourceManager()
        {
            Dispose();
        }

        private static JObject LoadFile(string path)
        {
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream))
            {
                var content = streamReader.ReadToEnd();
                return JObject.Parse(content);
            }
        }

        private void ResourceFileWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            var fileChanged = fileSystemEventArgs.ChangeType.HasFlag(WatcherChangeTypes.Changed);
            var fileRenamed = fileSystemEventArgs.ChangeType.HasFlag(WatcherChangeTypes.Renamed);
            var fileDeleted = fileSystemEventArgs.ChangeType.HasFlag(WatcherChangeTypes.Deleted);
            if (fileChanged || fileDeleted || fileRenamed)
            {
                var filePath = fileSystemEventArgs.FullPath;
                if (_resourceCache.TryGet(filePath, out var key))
                {
                    _resourceCache.Invalidate(_fileManager.GetResourceNameByPath(filePath));
                    _settings.ContentCache.Invalidate(filePath);
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
            }
            fileNameBuilder.Append(".json");
            return fileNameBuilder.ToString();
        }

        private JsonFileResource CreateResource(
            JObject content,
            string baseName,
            string location,
            string path,
            CultureInfo culture)
        {
            var resourceNameBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(location))
            {
                resourceNameBuilder.Append(location);
                resourceNameBuilder.Append(".");
            }
            resourceNameBuilder.Append(baseName);

            return new JsonFileResource(
                content,
                resourceNameBuilder.ToString(),
                path,
                culture,
                _loggerFactory.CreateLogger<JsonFileResource>(),
                _settings.ContentCache.GetContentCache(path));
        }

        /// <inheritdoc />
        public JsonFileResource GetResource(string baseName, string location, CultureInfo culture)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            baseName = baseName.Trim();
            location = location.Trim();

            string path = Path.Combine(_settings.ResourcesPath, GetFileName(baseName, location, culture));

            var pathFile = _fileManager.GetOrFindFile(path);
            if (pathFile != null)
            {
                return _resourceCache.GetOrAdd(path, key => CreateResource(LoadFile(pathFile), baseName, location, path, culture));
            }

            //try to find a file without a location in name
            if (String.IsNullOrWhiteSpace(location))
            {
                return null;
            }

            string noLocationPath = Path.Combine(_settings.ResourcesPath, GetFileName(baseName, String.Empty, culture));
            var noLocationPathFile = _fileManager.GetOrFindFile(noLocationPath);
            if (noLocationPathFile != null)
            {
                return _resourceCache.GetOrAdd(noLocationPath, key => CreateResource(LoadFile(noLocationPathFile), baseName, String.Empty, noLocationPath, culture));
            }

            return null;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _resourceFileWatcher?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
