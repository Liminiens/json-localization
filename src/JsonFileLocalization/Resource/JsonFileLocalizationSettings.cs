using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Localization settings for <see cref="JsonFileResourceManager"/>
    /// </summary>
    public class JsonFileLocalizationSettings : IJsonFileLocalizationSettings
    {
        /// <summary>
        /// Path to resource folder
        /// </summary>
        public string ResourcesPath { get; }

        /// <summary>
        /// Strategy for resource culture naming
        /// </summary>
        public JsonFileCultureSuffixStrategy CultureSuffixStrategy { get; }

        /// <summary>
        /// Creates a <see cref="JsonFileLocalizationSettings"/>
        /// </summary>
        /// <param name="environment">application environment service</param>
        /// <param name="cultureSuffixStrategy">Stratagy for culture name in resource file name</param>
        /// <param name="relativeResourcesPath">relative to content root path for resources</param>
        public JsonFileLocalizationSettings(
            IHostingEnvironment environment,
            JsonFileCultureSuffixStrategy cultureSuffixStrategy,
            string relativeResourcesPath)
        {
            CultureSuffixStrategy = cultureSuffixStrategy;
            ResourcesPath = Path.Combine(environment.ContentRootPath, relativeResourcesPath);
        }
    }
}
