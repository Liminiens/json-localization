using System;
using System.Collections.Concurrent;

namespace JsonFileLocalization.Caching
{
    internal class ConcurrentDictionaryCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _cache
            = new ConcurrentDictionary<TKey, Lazy<TValue>>();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return _cache.GetOrAdd(key, new Lazy<TValue>(valueFactory(key))).Value;
        }

        /// <summary>
        /// Invalidate value in cache by key
        /// </summary>
        /// <param name="key">key</param>
        public void Invalidate(TKey key)
        {
            _cache.TryRemove(key, out _);
        }

        /// <summary>
        /// Try add value to a cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>if added successfully</returns>
        public bool TryAdd(TKey key, TValue value)
        {
            return _cache.TryAdd(key, new Lazy<TValue>(value));
        }

        /// <summary>
        /// Try get value from a cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>if assigned value to a <paramref name="value"/></returns>
        public bool TryGet(TKey key, out TValue value)
        {
            if (_cache.TryGetValue(key, out var lazyValue))
            {
                value = lazyValue.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}