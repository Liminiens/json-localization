using System;
using Newtonsoft.Json.Linq;

namespace JsonFileLocalization.Caching
{
    public interface IJsonFileContentCache
    {
        JToken GetOrAdd(string key, Func<string, JToken> valueFactory);
    }
}