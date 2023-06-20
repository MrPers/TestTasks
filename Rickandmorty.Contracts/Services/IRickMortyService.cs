using Rickandmorty.DTO;

namespace Rickandmorty.Contracts.Services
{
    public interface IRickMortyService
    {
        Task<List<PersonFullInformationDto>> GetPersonsFullInformationAsync(string name);
        Task<bool> GetCheckPersonAsync(CheckPersonDto letter);
        Task<bool> SaveEpisodesAsync();
        int GetEpisodes(string episodeName);
    }
}
