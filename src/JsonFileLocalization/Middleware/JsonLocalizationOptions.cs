using JsonFileLocalization.Caching;
using JsonFileLocalization.Resource;

namespace JsonFileLocalization.Middleware
{
    /// <summary>
    /// Options for json localization services setup
    /// </summary>
    public class JsonLocalizationOptions
    {
        /// <summary>
        /// Path, relative to resources folder
        /// </summary>
        public string ResourceRelativePath { get; set; } = "Resources";

        /// <summary>
        /// Strategy for culture naming in file name
        /// </summary>
        public CultureSuffixStrategy CultureSuffixStrategy { get; set; } = CultureSuffixStrategy.TwoLetterISO6391AndCountryCode;

        /// <summary>
        /// Cache provider for localization files content
        /// </summary>
        public IJsonFileCacheProvider ContentCache { get; set; } = new JsonFileCacheProvider();

        /// <summary>
        /// Watch for file changes
        /// </summary>
        public bool WatchForChanges { get; set; } = false;
    }
}