using Microsoft.AspNetCore.Mvc;
using School_pws.Data.Entities;

namespace School_pws.Data
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        IQueryable GetAllWithUsers();

        bool SubjectExistsByCode(string code);

        Task ChangeActivedSubject(int id);
    }
}
