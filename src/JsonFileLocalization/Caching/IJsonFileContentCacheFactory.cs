namespace JsonFileLocalization.Caching
{
    /// <summary>
    /// Content cache factory
    /// </summary>
    public interface IJsonFileContentCacheFactory
    {
        /// <summary>
        /// Create cache instance
        /// </summary>
        /// <returns>instance of <see cref="IJsonFileContentCache"/></returns>
        IJsonFileContentCache Create();
    }
}