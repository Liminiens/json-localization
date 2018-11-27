using System;
using Newtonsoft.Json.Linq;

namespace JsonFileLocalization.Caching
{
    /// <summary>
    /// Service for caching json file content
    /// </summary>
    public interface IJsonFileContentCache
    {
        /// <summary>
        /// Get or add a new item to cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="valueFactory">value factory for key</param>
        /// <returns>value for this key</returns>
        object GetOrAdd(string key, Func<string, object> valueFactory);
    }
}