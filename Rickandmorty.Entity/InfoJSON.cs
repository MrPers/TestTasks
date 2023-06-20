using Newtonsoft.Json;

namespace Rickandmorty.Entity
{
    public class InfoJSON
    {

        [JsonProperty("next")]
        public string Next { get; set; }
    }
}