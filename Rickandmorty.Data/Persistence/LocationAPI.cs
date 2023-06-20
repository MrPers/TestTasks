
using AutoMapper;
using Newtonsoft.Json;
using Rickandmorty.Contracts.Persistence;
using Rickandmorty.DTO;
using Rickandmorty.Entity;

namespace Rickandmorty.Data
{
    public class LocationAPI : ILocationAPI
    {
        private readonly IMapper _mapper;

        public LocationAPI(
            IMapper mapper
        )
        {
            _mapper = mapper;
        }

        public async Task<LocationDto> GetLocationInformationAsync(string url)
        {
            var locations = await GetLocationsFromAPIAsync(url);

            return _mapper.Map<LocationDto>(locations);
        }
        private async Task<LocationJSON> GetLocationsFromAPIAsync(string url)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client
                .GetAsync(url);

            client.Dispose();

            if ((int)response.StatusCode != 200)
            {
                throw new ArgumentException(await response.Content.ReadAsStringAsync());
            }

            var locations = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LocationJSON>(locations);
        }
    }
}
