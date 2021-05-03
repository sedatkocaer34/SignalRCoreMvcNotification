using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Models
{
    public class UserRegisterViewModel:BaseViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email Adress is required.")]
        [EmailAddress(ErrorMessage="Email Adress Not Valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        public string PasswordMatch { get; set; }
    }
}
