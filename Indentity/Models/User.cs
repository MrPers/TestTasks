using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Indentity.Models
{
    public class User : IdentityUser<int>
    {
        public User(string userName) : base(userName)
        {
        }

        //[Column(TypeName = "varchar(50)")]
        //public override string UserName { get; set; }
        //public virtual ICollection<UserClaim> Claims { get; set; }
    }
}
