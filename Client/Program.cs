using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.MapControllers();
            //app.Run();

            while (true)
            {
                await Task.Delay(2000);
                await ServerRequest(app.Services.CreateScope());
            }
        }


        private static async Task ServerRequest(IServiceScope scope)
        {
            var scopeServiceProvider = scope.ServiceProvider;
            var httpClient = scopeServiceProvider.GetRequiredService<IHttpClientFactory>();

            // retrieve to Identity
            var authClient = httpClient.CreateClient();
            var discoveryDocument = await authClient.GetDiscoveryDocumentAsync("https://localhost:10001");

            var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,

                    ClientId = "client_id",
                    ClientSecret = "client_secret",

                    Scope = "OrdersAPI"
                });

            // retrieve to Server
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);
            var response = await client.GetAsync("https://localhost:1001/site/get-secrets");

            //if (!response.IsSuccessStatusCode)
            //{

            //}

            var messag = response.StatusCode.ToString();

            messag = await response.Content.ReadAsStringAsync();

            Console.WriteLine(messag);

            HttpContent httpContent = new StringContent("");
            response = await client.PostAsync("https://localhost:1001/site/post-secrets", httpContent);

            messag = await response.Content.ReadAsStringAsync();

            Console.WriteLine(messag);
        }

    }
}




