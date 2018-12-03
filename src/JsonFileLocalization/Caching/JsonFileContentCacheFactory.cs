using System;

namespace JsonFileLocalization.Caching
{
    /// <inheritdoc />
    public class JsonFileContentCacheFactory : IJsonFileContentCacheFactory
    {
        private readonly ConcurrentDictionaryCache<string, IJsonFileContentCache> _cacheProvider =
            new ConcurrentDictionaryCache<string, IJsonFileContentCache>();

        /// <inheritdoc />
        public IJsonFileContentCache Create()
        {
            return _cacheProvider.GetOrAdd(Guid.NewGuid().ToString(), key => new JsonFileContentCache());
        }
    }
}