namespace JsonFileLocalization.Caching
{
    public interface IJsonFileCacheProvider
    {
        IJsonFileContentCache GetContentCache(string cacheKey);
        void Invalidate(string cacheKey);
    }
}