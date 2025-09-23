using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Users
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
