namespace JsonFileLocalization.Caching
{
    public class JsonFileContentCacheFactory : IJsonFileContentCacheFactory
    {
        private readonly ConcurrentDictionaryCache<string, IJsonFileContentCache> _cacheProvider =
            new ConcurrentDictionaryCache<string, IJsonFileContentCache>();

        public IJsonFileContentCache GetContentCache(string cacheKey)
        {
            return _cacheProvider.GetOrAdd(cacheKey, key => new JsonFileContentCache());
        }

        public void Invalidate(string cacheKey)
        {
            _cacheProvider.Invalidate(cacheKey);
        }
    }
}