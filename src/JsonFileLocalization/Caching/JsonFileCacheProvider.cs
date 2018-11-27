using System.Collections.Concurrent;

namespace JsonFileLocalization.Caching
{
    public class JsonFileCacheProvider : IJsonFileCacheProvider
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