using Newtonsoft.Json;

namespace Rickandmorty.Entity
{
    public class LocationJSON
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("dimension")]
        public string Dimension { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}