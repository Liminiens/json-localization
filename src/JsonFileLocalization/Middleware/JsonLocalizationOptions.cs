using JsonFileLocalization.Resource;
using Microsoft.Extensions.Options;

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
        public string ResourceRelativePath { get; set; }

        /// <summary>
        /// Strategy for culture naming in file name
        /// </summary>
        public JsonFileCultureSuffixStrategy CultureSuffixStrategy { get; set; }
    }
}