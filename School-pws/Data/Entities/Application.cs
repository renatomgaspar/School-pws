using System.ComponentModel.DataAnnotations;

namespace School_pws.Data.Entities
{
    public class Application
    {
        public int Id { get; set; }


        [Required]
        public DateTime ApplicationDate { get; set; }


        [Required]
        public int? Grade { get; set; }


        [Required]
        public string Status { get; set; }


        [Required]
        public Student Student { get; set; }


        [Required]
        public Subject Subject { get; set; }
    }
}
