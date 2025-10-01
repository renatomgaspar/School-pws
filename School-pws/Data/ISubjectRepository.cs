using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_pws.Data.Entities;
using School_pws.Models.Applications;

namespace School_pws.Data
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<List<ApplicationDetailsViewModel>> GetUserSubjectsAsync(string email);

        bool SubjectExistsByCode(string code);

        Task ChangeActivedSubject(int id);

        IEnumerable<SelectListItem> GetComboSubjects();

        Task<bool> HasDependenciesAsync(Subject subject);
    }
}
