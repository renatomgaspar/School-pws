using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_pws.Data.Entities;

namespace School_pws.Data
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        IQueryable GetAllWithUsers();

        bool SubjectExistsByCode(string code);

        Task ChangeActivedSubject(int id);

        IEnumerable<SelectListItem> GetComboSubjects();
    }
}
