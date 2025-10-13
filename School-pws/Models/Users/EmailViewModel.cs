using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Users
{
    public class EmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
