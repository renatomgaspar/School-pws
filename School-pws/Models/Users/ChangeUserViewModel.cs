using System.ComponentModel.DataAnnotations;

namespace School_pws.Models.Users
{
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        public Guid? ImageId { get; set; }


        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44340/images/userDefaultImage.png"
            : $"https://schoolcontainer.blob.core.windows.net/users/{ImageId}";
    }
}
