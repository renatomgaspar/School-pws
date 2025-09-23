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
    }
}
