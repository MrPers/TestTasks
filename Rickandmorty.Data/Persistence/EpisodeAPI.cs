using AutoMapper;
using Newtonsoft.Json;
using Rickandmorty.Contracts.Persistence;
using Rickandmorty.DTO;
using Rickandmorty.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rickandmorty.Data
{
    public class EpisodeAPI : IEpisodeAPI
    {

        public async Task<Dictionary<string, int>> GetEpisodesInformationAsync()
        {
            var episodes = new Dictionary<string, int>();

            var series = await GetEpisodesFromAPIAsync();            
            episodes = episodes
                .Union(GetDictionaryEpisodes(series.Episodes))
                .ToDictionary(x => x.Key, x => x.Value);

            while (series.Info.Next is not null)
            {
                series = await GetEpisodesFromAPIAsync(series.Info.Next);
                episodes = episodes
                    .Union(GetDictionaryEpisodes(series.Episodes))
                    .ToDictionary(x => x.Key, x => x.Value);
            }

            return episodes;
        }

        private Dictionary<string, int> GetDictionaryEpisodes(List<EpisodeJSON> episodeJSONs)
        {
            var episodes = new Dictionary<string, int>();

            foreach (var episod in episodeJSONs)
            {
                episodes.TryAdd(episod.Name, episod.Id);
            }

            return episodes;
        }

        private async Task<SeriesJSON> GetEpisodesFromAPIAsync(string url = "https://rickandmortyapi.com/api/episode")
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client
                .GetAsync(url);

            client.Dispose();

            if ((int)response.StatusCode != 200)
            {
                throw new ArgumentException(await response.Content.ReadAsStringAsync());
            }

            var series = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SeriesJSON>(series);
        }
    }
}
