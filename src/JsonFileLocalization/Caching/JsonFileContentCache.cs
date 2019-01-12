namespace JsonFileLocalization.Caching
{
    internal class JsonFileContentCache : ConcurrentDictionaryCache<string, object>, IJsonFileContentCache
    {
    }
}
