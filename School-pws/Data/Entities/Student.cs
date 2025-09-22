using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace School_pws.Data.Entities
{
    public class Student : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
