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
    public class JsonFileResourceManager : IJsonFileResourceManager
    {
        private readonly IJsonFileLocalizationSettings _settings;
        private readonly ILoggerFactory _loggerFactory;

        private readonly ConcurrentDictionaryCache<string, JsonFileResource> _resourceCache
            = new ConcurrentDictionaryCache<string, JsonFileResource>();

        /// <summary>
        /// Creates an instance of resource manager with provided settings
        /// </summary>
        /// <param name="settings">settings for manager</param>
        /// <param name="loggerFactory">logger factory</param>
        public JsonFileResourceManager(
            IJsonFileLocalizationSettings settings,
            ILoggerFactory loggerFactory)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _loggerFactory = loggerFactory;
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
                case JsonFileCultureSuffixStrategy.TwoLetterISO6391:
                    fileNameBuilder.AppendFormat(".{0}", culture.TwoLetterISOLanguageName);
                    break;
                case JsonFileCultureSuffixStrategy.TwoLetterISO6391AndCountryCode:
                    fileNameBuilder.AppendFormat(".{0}", culture.Name);
                    break;
                default:
                    break;
            }
            fileNameBuilder.Append(".json");
            return fileNameBuilder.ToString();
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

        private JsonFileResource Create(string baseName, string location, CultureInfo culture)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }
            string path = Path.Combine(_settings.ResourcesPath, GetFileName(baseName.Trim(), location.Trim(), culture));
            if (File.Exists(path))
            {
                return new JsonFileResource(LoadFile(path), baseName, location,
                    path, culture, _loggerFactory.CreateLogger<JsonFileResource>());
            }
            //try to find a file without a location in name
            if (!String.IsNullOrWhiteSpace(location))
            {
                string noLocationPath = Path.Combine(_settings.ResourcesPath, GetFileName(baseName.Trim(), String.Empty, culture));

                if (File.Exists(noLocationPath))
                {
                    return new JsonFileResource(LoadFile(noLocationPath), baseName, String.Empty,
                        path, culture, _loggerFactory.CreateLogger<JsonFileResource>());
                }

                throw new JsonResourceFileNotFoundException(
                    $"Resource file with culture \"{culture.Name}\" not found on paths: \"{path}\" and \"{noLocationPath}\"");
            }

            throw new JsonResourceFileNotFoundException(
                $"Resource file with culture \"{culture.Name}\" not found: \"{path}\"");
        }

        /// <summary>
        /// Gets an already existing json resource or creates a new one if it doesnt exist yet
        /// </summary>
        /// <param name="baseName">name of a resource</param>
        /// <param name="location">assembly name or empty string</param>
        /// <param name="culture">resource culture</param>
        /// <returns>Json resource</returns>
        public JsonFileResource GetResource(string baseName, string location, CultureInfo culture)
        {
            string cacheKey = $"baseName={baseName};location={location};culture={culture.Name};";
            return _resourceCache.GetOrAdd(cacheKey, _ => Create(baseName, location, culture));
        }
    }
}
