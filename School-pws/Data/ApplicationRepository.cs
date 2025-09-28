using Microsoft.EntityFrameworkCore;
using School_pws.Data.Entities;
using School_pws.Helpers;
using School_pws.Migrations;
using School_pws.Models.Applications;
using System.Diagnostics;

namespace School_pws.Data
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ApplicationRepository(
            DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Application>> GetApplicationsAsync(string email)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
                {
                    return _context.Applications.OrderByDescending(a => a.ApplicationDate);
                }


                return _context.Applications.Where(a => a.User == user).OrderByDescending(a => a.ApplicationDate);
            }
        }

        public async Task<IQueryable<ApplicationDetails>> GetApplicationDetails(string email)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            return _context.ApplicationDetails
                .Include(a => a.Subject)
                .Where(a => a.User == user)
                .OrderBy(a => a.Subject.Name);
        }

        public async Task<bool> AddSubjectToApplication(AddApplicationViewModel model, string email)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var subject = await _context.Subjects.FindAsync(model.SubjectId);

            if (subject == null)
            {
                return false;
            }

            var applicationDetails = await _context.ApplicationDetails
                .Where(ad => ad.User == user && ad.Subject == subject)
                .FirstOrDefaultAsync();

            if (applicationDetails != null)
            {
                return false;
                
            }

            applicationDetails = new ApplicationDetails
            {
                Subject = subject,
                User = user,
                Grade = null,
                Status = "Applied"
            };

            _context.ApplicationDetails.Add(applicationDetails);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteSubjectFromApplication(int id)
        {
            var applicationDetails = await _context.ApplicationDetails.FindAsync(id);

            if (applicationDetails == null)
            {
                return;
            }

            _context.ApplicationDetails.Remove(applicationDetails);
            await _context.SaveChangesAsync();
        }
    }
}
