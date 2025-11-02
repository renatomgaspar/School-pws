using Microsoft.AspNetCore.Mvc;
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
                if (await _userHelper.IsUserInRoleAsync(user, "Admin") || await _userHelper.IsUserInRoleAsync(user, "Employee"))
                {
                    return _context.Applications
                        .Include(u => u.User)
                        .OrderByDescending(a => a.Status)
                        .OrderByDescending(a => a.ApplicationDate);
                }


                return _context.Applications.Where(a => a.User == user).OrderByDescending(a => a.ApplicationDate);
            }
        }

        public async Task<IQueryable<ApplicationDetailsTemp>> GetApplicationDetails(string email)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            return _context.ApplicationDetailsTemp
                .Include(a => a.Subject)
                .Where(a => a.User == user)
                .OrderBy(a => a.Subject.Name);
        }

        public async Task<ApplicationDetails> GetApplicationDetailsById(int id)
        {
            var applicationDetails = await _context.ApplicationDetails
                .Include(ad => ad.Subject)
                .Include(ad => ad.Application)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(ad => ad.Id == id);

            if (applicationDetails == null)
            {
                return null;
            }

            return applicationDetails;
        }

        public async Task UpdateApplicationDetailsAsync(ApplicationDetails applicationDetails)
        {
            if (applicationDetails.Grade > 9)
            {
                applicationDetails.Status = "Approved";
            }
            else
            {
                applicationDetails.Status = "Reproved";
            }

            _context.Update(applicationDetails);
            await _context.SaveChangesAsync();
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

            var applicationDetailsTemp = await _context.ApplicationDetailsTemp
                .Where(ad => ad.User == user && ad.Subject == subject)
                .FirstOrDefaultAsync();

            if (applicationDetailsTemp != null)
            {
                return false;
            }

            var existsSubjectInOtherApplication = await _context.Applications
                .AnyAsync(app => app.User == user
                && app.Subjects.Any(ad => ad.Subject == subject
                && (ad.Status == "Applied" || ad.Status == "On Going")));

            if (existsSubjectInOtherApplication)
            {
                return false;
            }

            applicationDetailsTemp = new ApplicationDetailsTemp
            {
                Subject = subject,
                User = user,
                Grade = null,
                Status = "Applied"
            };

            _context.ApplicationDetailsTemp.Add(applicationDetailsTemp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteSubjectFromApplication(int id)
        {
            var applicationDetailsTemp = await _context.ApplicationDetailsTemp.FindAsync(id);

            if (applicationDetailsTemp == null)
            {
                return;
            }

            _context.ApplicationDetailsTemp.Remove(applicationDetailsTemp);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmApplicationAsync(string email)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var applicationDetailsTemp = await _context.ApplicationDetailsTemp
                .Include(ad => ad.Subject)
                .Where(ad => ad.User == user)
                .ToListAsync();

            if (applicationDetailsTemp == null || applicationDetailsTemp.Count == 0)
            {
                return false;
            }

            var applicationDetails = applicationDetailsTemp.Select(ad => new ApplicationDetails
            {
                Subject = ad.Subject,
                Grade = null,
                Status = "Applied"
            }).ToList();

            var application = new Application
            {
                ApplicationDate = DateTime.UtcNow,
                User = user,
                Subjects = applicationDetails,
                Status = "Applied"
            };

            await CreateAsync(application);
            _context.ApplicationDetailsTemp.RemoveRange(applicationDetailsTemp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ApplicationDetailsViewModel>> GetApplicationDetailsAsync(int? id)
        {
            var application = await _context.Applications
                .Include(app => app.Subjects)
                .ThenInclude(ad => ad.Subject)
                .FirstOrDefaultAsync(app => app.Id == id);

            if (application == null)
            {
                return new List<ApplicationDetailsViewModel>();
            }

            return application.Subjects
                .Select(ad => new ApplicationDetailsViewModel
                {
                    Id = ad.Id,
                    ApplicationId = ad.Application.Id,
                    SubjectCode = ad.Subject.Code,
                    SubjectName = ad.Subject.Name,
                    Grade = ad.Grade,
                    Status = ad.Status
                })
                .ToList();
        }

        public async Task<bool> AcceptApplication(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Subjects)
                .ThenInclude(ad => ad.Subject)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return false;
            }

            application.Status = "Accepted";

            foreach (var subject in application.Subjects)
            {
                subject.Status = "On Going";
            }

            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DenyApplication(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Subjects)
                .ThenInclude(ad => ad.Subject)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return false;
            }

            application.Status = "Denied";

            foreach (var subject in application.Subjects)
            {
                subject.Status = "Canceled";
            }

            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
