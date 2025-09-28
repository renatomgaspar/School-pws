using School_pws.Data.Entities;
using School_pws.Migrations;
using School_pws.Models.Applications;

namespace School_pws.Data
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<IQueryable<Application>> GetApplicationsAsync(string email);

        Task<IQueryable<ApplicationDetails>> GetApplicationDetails(string email);

        Task<bool> AddSubjectToApplication(AddApplicationViewModel model, string email);

        Task DeleteSubjectFromApplication(int id);
    }
}
