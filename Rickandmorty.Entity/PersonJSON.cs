using Newtonsoft.Json;

namespace Rickandmorty.Entity
{
    public class PersonJSON 
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("species")]
        public string Species { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("origin")]
        public LocationJSON Location { get; set; }
        [JsonProperty("episode")]
        public List<string> Episode { get; set; }
    }
}