using System.ComponentModel.DataAnnotations;

namespace School_pws.Data.Entities
{
    public class Application : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Date")]
        public DateTime ApplicationDate { get; set; }


        [Required]
        [Display(Name = "Student")]
        public User User { get; set; }


        [Required]
        public IEnumerable<Subject> Subjects { get; set; }


        public int Lines => Subjects == null ? 0 : Subjects.Count();
    }
}
