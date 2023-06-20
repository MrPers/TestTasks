using Microsoft.Extensions.DependencyInjection;
using Rickandmorty.Contracts.Services;

namespace Rickandmorty.Logic
{
    public class DataSample
    {
        public static async Task InitializeAsync(IServiceScope serviceScope)
        {

            IServiceProvider scopeServiceProvider = serviceScope.ServiceProvider;
            var _rickMortyService = scopeServiceProvider.GetRequiredService<IRickMortyService>();

            await _rickMortyService.SaveEpisodesAsync();

        }
    }
}
