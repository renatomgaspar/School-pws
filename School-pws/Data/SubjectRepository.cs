using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_pws.Data.Entities;

namespace School_pws.Data
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly DataContext _context;

        public SubjectRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Subjects.Include(p => p.User);
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
            var list = _context.Subjects.Select(p => new SelectListItem
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
    }
}
