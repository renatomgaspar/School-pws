using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_pws.Data;
using School_pws.Helpers;
using School_pws.Models.Applications;
using System.Diagnostics;

namespace School_pws.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IConverterHelper _converterHelper;

        public ApplicationController(
            IApplicationRepository applicationRepository,
            ISubjectRepository subjectRepository,
            IConverterHelper converterHelper)
        {
            _applicationRepository = applicationRepository;
            _subjectRepository = subjectRepository;
            _converterHelper = converterHelper;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _applicationRepository.GetApplicationsAsync(this.User.Identity.Name);
            return View(model);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create()
        {
            var model = await _applicationRepository.GetApplicationDetails(this.User.Identity.Name);
            return View(model);
        }

        [Authorize(Roles = "Student")]
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
                ModelState.AddModelError(string.Empty, "This subject is already in your application list or is already in another application.");
                model.Subjects = _subjectRepository.GetComboSubjects();
                return View(model);
            }

            return Redirect("Create");
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> AddGrade(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationDetail = await _applicationRepository.GetApplicationDetailsById(id);
            if (applicationDetail == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToAddGradeViewModel(applicationDetail);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade(AddGradeViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            var applicationDetail = await _applicationRepository.GetApplicationDetailsById(model.Id);
            if (applicationDetail == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                applicationDetail.Grade = model.Grade;

                _applicationRepository.UpdateApplicationDetailsAsync(applicationDetail);

                return RedirectToAction("Details", new { id = applicationDetail.Application.Id }); ;
            }

            return View(model);
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

        public async Task<IActionResult> ConfirmApplication()
        {
            var response = await _applicationRepository.ConfirmApplicationAsync(this.User.Identity.Name);

            if (response)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _applicationRepository.GetApplicationDetailsAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        public async Task<IActionResult> AcceptApplication(int id)
        {
            var response = await _applicationRepository.AcceptApplication(id);

            if (response)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> DenyApplication(int id)
        {
            var response = await _applicationRepository.DenyApplication(id);

            if (response)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
