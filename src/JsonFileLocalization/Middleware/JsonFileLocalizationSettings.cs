using System.IO;
using JsonFileLocalization.Caching;
using JsonFileLocalization.Resource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace JsonFileLocalization.Middleware
{
    /// <summary>
    /// Localization settings for <see cref="JsonFileResourceManager"/>
    /// </summary>
    public class JsonFileLocalizationSettings
    {
        /// <summary>
        /// Path to resource folder
        /// </summary>
        public string ResourcesPath { get; }

        /// <summary>
        /// Strategy for resource culture naming
        /// </summary>
        public CultureSuffixStrategy CultureSuffixStrategy { get; }

        /// <summary>
        /// Cache provider for localization files content
        /// </summary>
        public IJsonFileContentCacheFactory ContentCacheFactory { get; }

        /// <summary>
        /// Watch for file changes
        /// </summary>
        public bool WatchForChanges { get; }

        /// <summary>
        /// Creates a <see cref="JsonFileLocalizationSettings"/>
        /// </summary>
        /// <param name="environment">application environment service</param>
        /// <param name="localizationOptions">localization options</param>
        public JsonFileLocalizationSettings(
            IHostingEnvironment environment,
            IOptions<JsonLocalizationOptions> localizationOptions)
        {
            var options = localizationOptions.Value ?? new JsonLocalizationOptions();
            ContentCacheFactory = options.ContentCacheFactory;
            CultureSuffixStrategy = options.CultureSuffixStrategy;
            WatchForChanges = options.WatchForChanges;
            ResourcesPath = Path.Combine(environment.ContentRootPath, options.ResourceRelativePath);
        }
    }
}
