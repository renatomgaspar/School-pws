using System.ComponentModel.DataAnnotations;

namespace School_pws.Data.Entities
{
    public class Subject
    {
        public int Id { get; set; }


        [Required]
        [MinLength(4, ErrorMessage = "The code must have at least 4 characters.")]
        [Display(Name = "Subject Code")]
        public string Code { get; set; }


        [Required]
        [MinLength(10, ErrorMessage = "The name must have at least 10 characters.")]
        [Display(Name = "Subject Name")]
        public string Name { get; set; }


        [Required]
        public string Description { get; set; }


        [Required]
        [Range(8, 1000, ErrorMessage = "The workload must be between 8 and 1000 hours.")]
        public int Workload { get; set; }


        [Required]
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
    }
}
