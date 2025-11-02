using School_pws.Data.Entities;
using School_pws.Migrations;
using School_pws.Models.Applications;

namespace School_pws.Data
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<IQueryable<Application>> GetApplicationsAsync(string email);

        Task<IQueryable<ApplicationDetailsTemp>> GetApplicationDetails(string email);

        Task<ApplicationDetails> GetApplicationDetailsById(int id);

        Task UpdateApplicationDetailsAsync(ApplicationDetails applicationDetails);

        Task<bool> AddSubjectToApplication(AddApplicationViewModel model, string email);

        Task DeleteSubjectFromApplication(int id);

        Task<bool> ConfirmApplicationAsync(string email);

        Task<List<ApplicationDetailsViewModel>> GetApplicationDetailsAsync(int? id);

        Task<bool> AcceptApplication(int id);

        Task<bool> DenyApplication(int id);
    }
}
