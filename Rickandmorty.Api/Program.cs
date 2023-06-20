using Microsoft.Extensions.Hosting;
using Rickandmorty.Contracts.Persistence;
using Rickandmorty.Contracts.Services;
using Rickandmorty.Data;
using Rickandmorty.Logic;
using Rickandmorty.Logic.Services;

namespace Rickandmorty.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(typeof(IRickMortyService), typeof(RickMortyService));
            builder.Services.AddScoped(typeof(IPersonAPI), typeof(PersonAPI));
            builder.Services.AddScoped(typeof(ILocationAPI), typeof(LocationAPI));
            builder.Services.AddScoped(typeof(IEpisodeAPI), typeof(EpisodeAPI));
            builder.Services.AddAutoMapper(typeof(Mapper));
            builder.Services.AddMemoryCache();

            // add services CORS
            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // add services CORS
            app.UseCors(builder => builder
                .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // request to an endpoint
            app.UseRouting();
            app.UseEndpoints(x => x.MapDefaultControllerRoute());

            SeedDataAsync(app);

            await app.RunAsync();
        }

        private static async void SeedDataAsync(WebApplication host)
        {
            using (var scope = host.Services.CreateScope())
            {
                await LogicSample.InitializeAsync(scope);
            }
        }
    }
}