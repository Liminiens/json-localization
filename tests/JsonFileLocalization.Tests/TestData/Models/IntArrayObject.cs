using Newtonsoft.Json;

namespace JsonFileLocalization.Tests.TestData.Models
{
    public class IntArrayObject
    {
        [JsonProperty("Value")]
        public int[] Value { get; set; }
    }
}