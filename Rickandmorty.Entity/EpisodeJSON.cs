using Newtonsoft.Json;

namespace Rickandmorty.Entity
{
    public class EpisodeJSON
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}