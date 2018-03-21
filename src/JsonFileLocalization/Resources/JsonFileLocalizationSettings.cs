using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace JsonFileLocalization.Resources
{
    public class JsonFileLocalizationSettings : IJsonFileLocalizationSettings
    {
        public string ResourcesPath { get; }

        public JsonFileCultureSuffixStrategy CultureSuffixStrategy { get; }

        /// <summary>
        /// Creates a <see cref="JsonFileLocalizationSettings"/> instance with resource path relative to content root
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
