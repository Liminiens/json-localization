using Newtonsoft.Json;

namespace JsonFileLocalization.Tests.TestData.Models
{
    public class TestObject
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}