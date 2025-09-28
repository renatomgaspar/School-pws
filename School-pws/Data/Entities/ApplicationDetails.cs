using System.ComponentModel.DataAnnotations;

namespace School_pws.Data.Entities
{
    public class ApplicationDetails
    {
        public int Id { get; set; }


        [Required]
        public User User { get; set; }


        [Required]
        public Subject Subject { get; set; }


        public int? Grade { get; set; }


        [Required]
        public string Status { get; set; }
    }
}
