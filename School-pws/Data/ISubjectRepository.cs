using School_pws.Data.Entities;

namespace School_pws.Data
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public IQueryable GetAllWithUsers();

        bool SubjectExistsByCode(string code);
    }
}
