using System.Globalization;

namespace JsonFileLocalization.Resources
{
    /// <summary>
    /// Strategy for culture naming in resource file name
    /// </summary>
    public enum JsonFileCultureSuffixStrategy
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Use two letter name of culture as in ISO639-1
        /// </summary>
        /// <example>
        /// Example: "ru", "en", "es"
        /// </example>
        TwoLetterISO6391,
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Use Name property of <see cref="CultureInfo"/> object which is ISO639-1 + country/region code
        /// </summary>
        /// <example>
        /// Example: "ru-RU" or "en-US"
        /// </example>
        TwoLetterISO6391AndCountryCode
    }
}