namespace JsonFileLocalization.Caching
{
    /// <inheritdoc />
    internal class JsonFileContentCacheFactory : IJsonFileContentCacheFactory
    {
        /// <inheritdoc />
        public IJsonFileContentCache Create()
        {
            return new JsonFileContentCache();
        }
    }
}