using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_UOC_2024.APP.Model
{
    public class LoginModel
    {
        [Display(Prompt = "example@mail.com", Name = "Email")]
        [EmailAddress(ErrorMessage = "Enter your email - example@mail.com")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter the password")]
        public string Password { get; set; }
    }
}
