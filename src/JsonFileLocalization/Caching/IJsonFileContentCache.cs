using System;
using Newtonsoft.Json.Linq;

namespace JsonFileLocalization.Caching
{
    /// <summary>
    /// Service for caching JPath results
    /// </summary>
    public interface IJsonFileContentCache
    {
        /// <summary>
        /// Get or add a new JPath result to cache
        /// </summary>
        /// <param name="key">JPath string</param>
        /// <param name="valueFactory">value factory for <see cref="JToken"/></param>
        /// <returns>A <see cref="JToken"/> from cache on this JPath</returns>
        JToken GetOrAdd(string key, Func<string, JToken> valueFactory);
    }
}