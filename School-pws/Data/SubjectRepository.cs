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

        public void AddSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
        }

        public Subject GetSubject(int id)
        {
            return _context.Subjects.Find(id);
        }

        public IEnumerable<Subject> GetSubjects()
        {
            return _context.Subjects.OrderBy(s => s.Name);
        }

        public void RemoveSubject(Subject subject)
        {
            _context.Subjects.Remove(subject);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool SubjectExists(int id)
        {
            return _context.Subjects.Any(s => s.Id == id);
        }

        public bool SubjectExistsByCode(string code)
        {
            return _context.Subjects.Any(s => s.Code == code);
        }

        public void UpdateSubject(Subject subject)
        {
            _context.Subjects.Update(subject);
        }
    }
}
