namespace JsonFileLocalization.Resource
{
    /// <summary>
    /// Provides settings for json file localization
    /// </summary>
    public interface IJsonFileLocalizationSettings
    {
        /// <summary>
        /// Culture suffix strategy
        /// </summary>
        JsonFileCultureSuffixStrategy CultureSuffixStrategy { get; }

        /// <summary>
        /// Path to application resources
        /// </summary>
        string ResourcesPath { get; }
    }
}