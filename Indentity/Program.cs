using Duende.IdentityServer.Services;
using Indentity.Data;
using Indentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Indentity
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services);

            var host = builder.Build();
            Configure(host, builder.Environment);
            //await SeedDatabaseAsync(host);
            await host.RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            //services.AddHttpClient(); // to send requests

            //services.AddDbContext<DataContext>(config =>
            //{
            //    // When using real SQL server
            //    config.UseInMemoryDatabase("DEMO_ONLY");
            //})
            //    .AddIdentity<User, Role>(config =>
            //    {
            //        //config.Password.RequireDigit = false;
            //        //config.Password.RequireLowercase = false;
            //        //config.Password.RequireNonAlphanumeric = false;
            //        //config.Password.RequireUppercase = false;
            //        //config.Password.RequiredLength = 6;
            //    })
            //    .AddEntityFrameworkStores<DataContext>();

            //services.ConfigureApplicationCookie(config =>
            //{
            //    config.LoginPath = "/Auth/Login";
            //    config.LogoutPath = "/Auth/Logout";
            //    config.Cookie.Name = "IdentityServer.Cookies";
            //});

            services.AddIdentityServer()
                .AddInMemoryClients(IdentityServerConfiguration.GetClients())
                .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources())
                .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                //.AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
                .AddDeveloperSigningCredential();

            //services.AddSingleton<ICorsPolicyService>((container) =>
            //{
            //    var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();     //CORE от IS4
            //    return new DefaultCorsPolicyService(logger)
            //    {
            //        AllowedOrigins = { "http://localhost:4200" }
            //    };
            //});


            services.AddControllersWithViews();

            //services.AddControllersWithViews()
            //    .AddRazorRuntimeCompilation();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//Captures synchronous and asynchronous Exception instances from the pipeline and generates HTML error responses.
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static async Task SeedDatabaseAsync(WebApplication host)
        {

            using (var scope = host.Services.CreateScope())
            {
                await DatabaseInitializer.InitializeAsync(scope);
            }
        }
    }
}

