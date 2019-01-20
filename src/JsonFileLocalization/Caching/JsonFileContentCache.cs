using System;
using System.Collections.Concurrent;

namespace JsonFileLocalization.Caching
{
    internal class JsonFileContentCache : IJsonFileContentCache
    {
        private readonly ConcurrentDictionary<string, Lazy<object>> _cache
               = new ConcurrentDictionary<string, Lazy<object>>();

        public object GetOrAdd(string key, Func<string, object> valueFactory)
        {
            return _cache.GetOrAdd(key, new Lazy<object>(() => valueFactory(key))).Value;
        }
    }
}
