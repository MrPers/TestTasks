using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
//using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel;
using Indentity.Data;
using Indentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Indentity
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceScope serviceScope)
        {
            IServiceProvider scopeServiceProvider = serviceScope.ServiceProvider;

            //scopeServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
            //scopeServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            //var dataContext = scopeServiceProvider.GetService<DataContext>();
            //var configurationContext = scopeServiceProvider.GetRequiredService<ConfigurationDbContext>();

            //if (!configurationContext.Clients.Any() && !configurationContext.ApiResources.Any())
            //{
            //    foreach (var client in IdentityServerConfiguration.GetClients())
            //    {
            //        configurationContext.Clients.Add(client.ToEntity());
            //    }

            //    foreach (var resource in IdentityServerConfiguration.GetIdentityResources())
            //    {
            //        configurationContext.IdentityResources.Add(resource.ToEntity());
            //    }

            //    foreach (var resource in IdentityServerConfiguration.GetApiResources())
            //    {
            //        configurationContext.ApiResources.Add(resource.ToEntity());
            //    }

            //    configurationContext.SaveChanges();
            //}

            var userManager = scopeServiceProvider.GetService<UserManager<User>>();
            var roleManager = scopeServiceProvider.GetService<RoleManager<Role>>();

            var users = new User[] {
                new User("AdministratorALL"),
                new User("User"),
            };

            userManager.CreateAsync(users[0], "12qw!Q").GetAwaiter().GetResult();
            userManager.CreateAsync(users[1], "12qw!Q").GetAwaiter().GetResult();

            var roles = new Role[] {
                new Role("Administrator"),
                new Role("User")
            };

            roleManager.CreateAsync(roles[0]).GetAwaiter().GetResult();
            roleManager.CreateAsync(roles[1]).GetAwaiter().GetResult();


            //userManager.AddClaimAsync(users[0], new Claim(ClaimTypes.Role, "Coin")).GetAwaiter().GetResult();
            //userManager.AddClaimAsync(users[0], new Claim(ClaimTypes.Role, "Letter")).GetAwaiter().GetResult();
            //userManager.AddClaimAsync(users[0], new Claim(JwtClaimTypes.Scope, "OrdersAPI")).GetAwaiter().GetResult();
            //userManager.AddClaimAsync(users[1], new Claim(ClaimTypes.Role, "Letter")).GetAwaiter().GetResult();
            //userManager.AddClaimAsync(users[1], new Claim(JwtClaimTypes.Scope, "OrdersAPI")).GetAwaiter().GetResult();

            var claims = new RoleClaim[] {
                new RoleClaim{ ClaimType = JwtRegisteredClaimNames.Jti, ClaimValue = "Coin" },
                new RoleClaim{ ClaimType = JwtRegisteredClaimNames.Jti, ClaimValue = "Letter" },
            };

            roleManager.AddClaimAsync(roles[0], claims[0].ToClaim()).GetAwaiter();
            roleManager.AddClaimAsync(roles[0], claims[1].ToClaim()).GetAwaiter();
            roleManager.AddClaimAsync(roles[1], claims[1].ToClaim()).GetAwaiter();
        }
    }
}