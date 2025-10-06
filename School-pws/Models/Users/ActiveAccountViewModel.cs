using School_pws.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Users
{
    public class ActiveAccountViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(6)]
        [Display(Name = "Password")]
        public string NewPassword { get; set; }


        [Required]
        [Compare("NewPassword")]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
    }
}
