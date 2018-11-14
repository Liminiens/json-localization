using System.IO;
using JsonFileLocalization.Middleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace JsonFileLocalization.Resource
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
        /// Creates a <see cref="JsonFileLocalizationSettings"/>
        /// </summary>
        /// <param name="environment">application environment service</param>
        /// <param name="options">localization options</param>
        public JsonFileLocalizationSettings(
            IHostingEnvironment environment,
            IOptions<JsonLocalizationOptions> options)
        {
            CultureSuffixStrategy = options.Value.CultureSuffixStrategy;
            ResourcesPath = Path.Combine(environment.ContentRootPath, options.Value.ResourceRelativePath);
        }
    }
}
