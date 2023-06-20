using Rickandmorty.DTO;

namespace Rickandmorty.Contracts.Persistence
{
    public interface IEpisodeAPI
    {
        Task<Dictionary<string, int>> GetEpisodesInformationAsync();
    }
}
