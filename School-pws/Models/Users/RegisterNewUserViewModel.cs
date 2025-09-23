using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Users
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }


        [Display(Name = "UserName*")]
        public string? UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }


        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }


        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
