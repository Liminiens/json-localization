using Newtonsoft.Json;

namespace JsonFileLocalization.Tests.TestData.Models
{
    public class LocationTest
    {
        [JsonProperty("TestArray")]
        public int[] TestArray { get; set; }
    }
}