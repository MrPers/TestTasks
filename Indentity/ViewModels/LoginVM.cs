using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Indentity.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MinLength(4)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string ReturnUrl { get; set; }

        //public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
    }
}