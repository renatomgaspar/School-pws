using School_pws.Data.Entities;

namespace School_pws.Data
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        IEnumerable<Subject> GetSubjects();
        Subject GetSubject(int id);
        bool SubjectExists(int id);
        bool SubjectExistsByCode(string code);
        void AddSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void RemoveSubject(Subject subject);
        Task<bool> SaveAllAsync();
    }
}
