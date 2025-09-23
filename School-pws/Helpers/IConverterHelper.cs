using School_pws.Data.Entities;
using School_pws.Models.Users;

namespace School_pws.Helpers
{
    public interface IConverterHelper
    {
        User ToStudent(RegisterNewUserViewModel model, Guid imageId, bool isNew);

        RegisterNewUserViewModel ToRegisterNewStudentViewModel(User user);
    }
}
