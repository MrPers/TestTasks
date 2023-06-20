using Rickandmorty.DTO;

namespace Rickandmorty.Contracts.Persistence
{
    public interface IPersonAPI
    {
        Task<List<PersonFullInformationDto>> GetPersonsFullInformationAsync(string name);
        Task<HashSet<int>> GetePisodesWithThisPersonAsync(string name);
    }
}
