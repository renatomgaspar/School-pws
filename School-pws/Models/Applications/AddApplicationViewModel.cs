using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Applications
{
    public class AddApplicationViewModel
    {
        [Display(Name = "Subject")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Subject.")]
        public int SubjectId { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }
    }
}
