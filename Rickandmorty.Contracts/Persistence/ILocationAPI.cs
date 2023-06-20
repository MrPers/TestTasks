using Rickandmorty.DTO;

namespace Rickandmorty.Contracts.Persistence
{
    public interface ILocationAPI
    {
        Task<LocationDto> GetLocationInformationAsync(string url);
    }
}
