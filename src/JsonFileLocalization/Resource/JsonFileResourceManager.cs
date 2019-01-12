using JsonFileLocalization.Caching;
using JsonFileLocalization.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Resource manager based on JSON files
    /// </summary>
    public class JsonFileResourceManager : IJsonFileResourceManager, IDisposable
    {
        private readonly FileSystemWatcher _resourceFileWatcher;
        private readonly FilePathCache _filePathCache = new FilePathCache();

        private readonly JsonFileLocalizationSettings _settings;
        private readonly ConcurrentDictionaryCache<string, JsonFileResource> _resourceCache
            = new ConcurrentDictionaryCache<string, JsonFileResource>();

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<JsonFileResourceManager> _logger;

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
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size,
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
            if (fileChanged)
            {
                var filePath = fileSystemEventArgs.FullPath;
                _resourceCache.Invalidate(filePath);
                _logger.LogInformation("Deleted resource \"{path}\" from cache", filePath);
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
                _settings.ContentCacheFactory);
        }

        /// <inheritdoc />
        public JsonFileResource GetResource(string baseName, string location, CultureInfo culture)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            baseName = baseName.Trim();
            location = location?.Trim() ?? String.Empty;

            var path = _filePathCache.GetOrFindFile(Path.Combine(_settings.ResourcesPath, GetFileName(baseName, location, culture)));
            if (path != null)
            {
                return _resourceCache.GetOrAdd(path, key => CreateResource(LoadFile(path), baseName, location, path, culture));
            }

            //try to find a file without a location in name
            if (String.IsNullOrWhiteSpace(location))
            {
                return null;
            }

            var noLocationPath = _filePathCache.GetOrFindFile(Path.Combine(_settings.ResourcesPath, GetFileName(baseName, String.Empty, culture)));
            if (noLocationPath != null)
            {
                return _resourceCache.GetOrAdd(noLocationPath, key => CreateResource(LoadFile(noLocationPath), baseName, String.Empty, noLocationPath, culture));
            }

            return null;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_resourceFileWatcher != null)
            {
                _resourceFileWatcher.Changed -= ResourceFileWatcherOnChanged;
                _resourceFileWatcher.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
