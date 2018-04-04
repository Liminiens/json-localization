using Newtonsoft.Json.Linq;

namespace JsonFileLocalization.Caching
{
    /// <summary>
    /// Service for storing JPath results in a <see cref="ConcurrentDictionaryCache{TKey,TValue}"/>
    /// </summary>
    public class JsonFileContentCache : ConcurrentDictionaryCache<string, JToken>, IJsonFileContentCache
    {
    }
}
