using School_pws.Data.Entities;
using School_pws.Models.Users;

namespace School_pws.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public RegisterNewUserViewModel ToRegisterNewStudentViewModel(User user)
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

        public User ToStudent(RegisterNewUserViewModel model, Guid imageId, bool isNew)
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
    }
}
