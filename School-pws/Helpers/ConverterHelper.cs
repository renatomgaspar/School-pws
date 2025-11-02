using School_pws.Data.Entities;
using School_pws.Models.Applications;
using School_pws.Models.Users;

namespace School_pws.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public RegisterNewUserViewModel ToRegisterNewUserViewModel(User user)
        {
            return new RegisterNewUserViewModel
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageFile = null
            };
        }

        public User ToUser(RegisterNewUserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = imageId,
            };
        }

        public AddGradeViewModel ToAddGradeViewModel(ApplicationDetails applicationDetails)
        {
            return new AddGradeViewModel
            {
                Id = applicationDetails.Id,
                Grade = applicationDetails.Grade,
                Status = applicationDetails.Status,
                Application = applicationDetails.Application,
                Subject = applicationDetails.Subject
            };
        }

        public ApplicationDetails ToApplicationDetails(AddGradeViewModel model, Guid imageId, bool isNew)
        {
            return new ApplicationDetails
            {
                Grade = model.Grade,
                Status = model.Status,
                Application = model.Application,
                Subject = model.Subject
            };
        }
    }
}
