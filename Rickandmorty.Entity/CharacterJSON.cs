using Newtonsoft.Json;

namespace Rickandmorty.Entity
{
    public class CharacterJSON
    {
        [JsonProperty("results")]
        public List<PersonJSON> Persons { get; set; }
    }
}
