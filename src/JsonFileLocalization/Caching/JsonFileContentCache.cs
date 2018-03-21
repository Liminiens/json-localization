using Newtonsoft.Json.Linq;

namespace JsonFileLocalization.Caching
{
    public class JsonFileContentCache : ConcurrentDictionaryCache<string, JToken>, IJsonFileContentCache
    {
    }
}
