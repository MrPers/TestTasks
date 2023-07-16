using Indentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Indentity.Data
{

    public class DataContext: IdentityDbContext<
    User, // TUser
    Role, // TRole
    int, // TKey
    IdentityUserClaim<int>, // TUserClaim
    IdentityUserRole<int>, // TUserRole,
    IdentityUserLogin<int>, // TUserLogin
    RoleClaim, // TRoleClaim
    IdentityUserToken<int> // TUserToken
    >
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {

        }

    }
}
