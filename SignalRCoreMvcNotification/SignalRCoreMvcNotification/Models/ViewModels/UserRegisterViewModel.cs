using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Models
{
    public class UserRegisterViewModel:BaseViewModel
    {
        [Required(ErrorMessage = "Email zorunludur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "User Name zorunludur")]
        [EmailAddress(ErrorMessage="Email Adress Not Valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre Tekrar zorunludur")]
        public string PasswordMatch { get; set; }
    }
}
