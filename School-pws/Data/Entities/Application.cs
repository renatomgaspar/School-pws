using System.ComponentModel.DataAnnotations;

namespace School_pws.Data.Entities
{
    public class Application : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Application Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime ApplicationDate { get; set; }


        [Required]
        [Display(Name = "Student")]
        public User User { get; set; }


        [Required]
        public IEnumerable<ApplicationDetails> Subjects { get; set; }


        [Required]
        public string Status { get; set; }


        public int Lines => Subjects == null ? 0 : Subjects.Count();
    }
}
