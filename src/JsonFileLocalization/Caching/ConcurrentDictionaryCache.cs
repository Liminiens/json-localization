using System;
using System.Collections.Concurrent;

namespace JsonFileLocalization.Caching
{
    /// <summary>
    /// Permanent cache based on <see cref="ConcurrentDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public class ConcurrentDictionaryCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _cache
            = new ConcurrentDictionary<TKey, Lazy<TValue>>();

        /// <summary>
        /// Get or add value by key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="valueFactory">value factory which is called when there is no value</param>
        /// <returns></returns>
        public virtual TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return _cache.GetOrAdd(key, new Lazy<TValue>(valueFactory(key))).Value;
        }
    }
}