using AutoMapper;
using Newtonsoft.Json;
using Rickandmorty.Contracts.Persistence;
using Rickandmorty.DTO;
using Rickandmorty.Entity;

namespace Rickandmorty.Data
{
    public class PersonAPI : IPersonAPI
    {
        private readonly IMapper _mapper;
        private readonly string _url = "https://rickandmortyapi.com/api";

        public PersonAPI(
            IMapper mapper
        )
        {
            _mapper = mapper;
        }

        public async Task<List<PersonFullInformationDto>> GetPersonsFullInformationAsync(string name)
        {
            var characters = await GetPersonsFromAPIAsync(name);

            return _mapper.Map<List<PersonFullInformationDto>>(characters.Persons);
        }

        public async Task<HashSet<int>> GetePisodesWithThisPersonAsync(string name)
        {
            var episodesWithThisPerson = new HashSet<int>();
            var characters = await GetPersonsFromAPIAsync(name);

            foreach (var person in characters.Persons)
            {
                foreach (var episod in person.Episode)
                {
                    episodesWithThisPerson.Add(Convert.ToInt32(episod.Remove(0, episod.LastIndexOf('/') + 1)));
                }
            }

            return episodesWithThisPerson;
        }

        private async Task<CharacterJSON> GetPersonsFromAPIAsync(string name)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client
                .GetAsync($"{_url}/character/?name={name}");

            client.Dispose();

            if ((int)response.StatusCode != 200)
            {
                throw new ArgumentException(await response.Content.ReadAsStringAsync());
            }
            var characters = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CharacterJSON>(characters);
        }
    }
}
