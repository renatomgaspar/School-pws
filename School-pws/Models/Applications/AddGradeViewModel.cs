using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using School_pws.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Applications
{
    public class AddGradeViewModel
    {
        public int Id { get; set; }

        [Range(0, 20, ErrorMessage = "Grade must be between 0 and 20")]
        public float? Grade { get; set; }

        [ValidateNever]
        public string Status { get; set; }

        [ValidateNever]
        public Application Application { get; set; }

        [ValidateNever]
        public Subject Subject { get; set; }
    }
}
