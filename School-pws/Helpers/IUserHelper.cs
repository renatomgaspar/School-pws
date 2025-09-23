using Microsoft.AspNetCore.Identity;
using School_pws.Data.Entities;

namespace School_pws.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddStudentAsync(User student, string password);
    }
}
