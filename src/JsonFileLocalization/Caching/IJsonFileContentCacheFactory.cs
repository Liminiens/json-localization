namespace JsonFileLocalization.Caching
{
    /// <summary>
    /// Content cache factory
    /// </summary>
    public interface IJsonFileContentCacheFactory
    {
        /// <summary>
        /// Create or get content cache
        /// </summary>
        /// <param name="cacheKey">cache key</param>
        /// <returns>instance of <see cref="IJsonFileContentCache"/></returns>
        IJsonFileContentCache GetContentCache(string cacheKey);

        /// <summary>
        /// Invalidates cache by key
        /// </summary>
        /// <param name="cacheKey">cache key</param>
        void Invalidate(string cacheKey);
    }
}