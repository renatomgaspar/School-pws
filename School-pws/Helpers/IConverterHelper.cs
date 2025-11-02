using School_pws.Data.Entities;
using School_pws.Models.Applications;
using School_pws.Models.Users;

namespace School_pws.Helpers
{
    public interface IConverterHelper
    {
        User ToUser(RegisterNewUserViewModel model, Guid imageId, bool isNew);

        RegisterNewUserViewModel ToRegisterNewUserViewModel(User user);

        AddGradeViewModel ToAddGradeViewModel(ApplicationDetails applicationDetails);
    }
}
