using Microsoft.Extensions.Caching.Memory;
using Rickandmorty.Contracts.Persistence;
using Rickandmorty.Contracts.Services;
using Rickandmorty.DTO;

namespace Rickandmorty.Logic.Services
{
    public class RickMortyService : IRickMortyService
    {
        private readonly IPersonAPI _personAPI;
        private readonly ILocationAPI _locationAPI;
        private readonly IEpisodeAPI _episodeAPI;
        private readonly IMemoryCache cache;
        private readonly string notFound = "not found";

        public RickMortyService(
            IEpisodeAPI episodeAPI,
            IPersonAPI personAPI,
            ILocationAPI locatioAPI,
            IMemoryCache memoryCache
        )
        {
            cache = memoryCache;
            _episodeAPI = episodeAPI;
            _personAPI = personAPI;
            _locationAPI = locatioAPI;
        }

        public async Task<List<PersonFullInformationDto>> GetPersonsFullInformationAsync(string name)
        {
            if (cache.TryGetValue(name, out List<PersonFullInformationDto> persons))
            {
                return persons;
            }

            if (cache.Get(name) == notFound)
            {
                throw new Exception();
            }

            try
            {
                persons = await GetPersonsInformationAsync(name, persons);
                cache.Set(name, persons, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

                return persons;
            }
            catch (Exception ex)
            {
                cache.Set(name, notFound, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> GetCheckPersonAsync(CheckPersonDto checkPerso)
        {
            var name = checkPerso.EpisodeName + checkPerso.PersonName;

            if (cache.TryGetValue(name, out bool status))
            {
                return status;
            }

            if (cache.Get(name) == notFound)
            {
                throw new Exception();
            }

            try
            {
                status = await CheckPersonAsync(checkPerso, status);

                cache.Set(checkPerso.EpisodeName + checkPerso.PersonName, status, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

                return status;
            }
            catch (Exception ex)
            {
                cache.Set(name, notFound, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SaveEpisodesAsync()
        {
            var episodes = await _episodeAPI.GetEpisodesInformationAsync();

            foreach (var episode in episodes)
            {
                cache.Set(episode.Key, episode.Value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
            }

            return true;
        }

        public int GetEpisodes(string episodeName)
        {
            int episodeId;

            if (!cache.TryGetValue(episodeName, out episodeId))
            {
                throw new Exception(notFound);
            }

            return episodeId;
        }

        private async Task<List<PersonFullInformationDto>> GetPersonsInformationAsync(string name, List<PersonFullInformationDto> persons)
        {
            persons = await _personAPI.GetPersonsFullInformationAsync(name);

            foreach (var person in persons)
            {
                if (person.Location.Url != "")
                {
                    person.Location = await _locationAPI.GetLocationInformationAsync(person.Location.Url);
                }
            }

            return persons;
        }

        private async Task<bool> CheckPersonAsync(CheckPersonDto checkPerso, bool status)
        {
            var episodesWithThisPerson = await _personAPI.GetePisodesWithThisPersonAsync(checkPerso.PersonName);
            var value = GetEpisodes(checkPerso.EpisodeName);
            status = episodesWithThisPerson.TryGetValue(value, out value);
            return status;
        }
    }
}
