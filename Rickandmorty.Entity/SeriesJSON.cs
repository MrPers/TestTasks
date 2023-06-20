using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rickandmorty.Entity
{
    public class SeriesJSON
    {
        [JsonProperty("info")]
        public InfoJSON Info { get; set; }

        [JsonProperty("results")]
        public List<EpisodeJSON> Episodes { get; set; }

        public static implicit operator string(SeriesJSON v)
        {
            throw new NotImplementedException();
        }
    }
}
