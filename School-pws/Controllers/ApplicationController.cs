using Microsoft.AspNetCore.Mvc;
using School_pws.Data;
using School_pws.Models.Applications;
using System.Diagnostics;

namespace School_pws.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ISubjectRepository _subjectRepository;

        public ApplicationController(
            IApplicationRepository applicationRepository,
            ISubjectRepository subjectRepository)
        {
            _applicationRepository = applicationRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _applicationRepository.GetApplicationsAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _applicationRepository.GetApplicationDetails(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddSubject()
        {
            var model = new AddApplicationViewModel
            {
                Subjects = _subjectRepository.GetComboSubjects()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(AddApplicationViewModel model)
        {
            var response = await _applicationRepository.AddSubjectToApplication(model, this.User.Identity.Name);

            if (!response)
            {
                ModelState.AddModelError(string.Empty, "This subject is already in your application list.");
                model.Subjects = _subjectRepository.GetComboSubjects();
                return View(model);
            }

            return Redirect("Create");
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _applicationRepository.DeleteSubjectFromApplication(id.Value);

            return RedirectToAction("Create");
        }
    }
}
