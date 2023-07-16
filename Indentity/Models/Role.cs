using Microsoft.AspNetCore.Identity;

namespace Indentity.Models
{
    public class Role: IdentityRole<int>
    {
        public Role(string roleName) : base(roleName)
        {
        }

        public Role()
        {
        }
    }
}
