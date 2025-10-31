using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_pws.Data.Entities;
using School_pws.Helpers;
using School_pws.Models.Applications;

namespace School_pws.Data
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SubjectRepository(
            DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<List<ApplicationDetailsViewModel>> GetUserSubjectsAsync(string email)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new List<ApplicationDetailsViewModel>();
            }

            return await _context.Applications
                .Include(a => a.Subjects)
                .ThenInclude(ad => ad.Subject)
                .Where(a => a.User == user)
                .SelectMany(a => a.Subjects)
                .Where(ad => ad.Status == "On Going")
                .Select(ad => new ApplicationDetailsViewModel
                {
                    SubjectCode = ad.Subject.Code,
                    SubjectName = ad.Subject.Name,
                    Grade = ad.Grade,
                    Status = ad.Status
                })
                .ToListAsync();
        }

        public bool SubjectExistsByCode(string code)
        {
            return _context.Subjects.Any(s => s.Code == code);
        }

        public async Task ChangeActivedSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);

            if (subject == null)
            {
                return;
            }

            if (subject.IsActive)
            {
                subject.IsActive = false;
            }
            else
            {
                subject.IsActive = true;
            }

            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<SelectListItem> GetComboSubjects()
        {
            var list = _context.Subjects
                .Where(s => s.IsActive == true)
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();


            list.Insert(0, new SelectListItem
            {
                Text = "(Select a subject...)",
                Value = "0"
            });

            return list;
        }

        public async Task<bool> HasDependenciesAsync(Subject subject)
        {
            return await _context.ApplicationDetails.AnyAsync(ad => ad.Subject == subject) 
                || await _context.ApplicationDetailsTemp.AnyAsync(adt => adt.Subject == subject);
        }
    }
}
