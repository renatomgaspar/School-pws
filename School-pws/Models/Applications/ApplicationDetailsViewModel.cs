using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Applications
{
    public class ApplicationDetailsViewModel
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; }


        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }


        public float? Grade { get; set; }


        public string Status { get; set; }
    }
}
