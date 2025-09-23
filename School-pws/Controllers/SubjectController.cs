using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_pws.Data;
using School_pws.Data.Entities;
using School_pws.Helpers;

namespace School_pws.Controllers
{
    public class SubjectController : Controller
    {
        private ISubjectRepository _subjectRepository;
        private readonly IUserHelper _userHelper;

        public SubjectController(
            ISubjectRepository subjectRepository,
            IUserHelper userHelper)
        {
            _subjectRepository = subjectRepository;
            _userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View(_subjectRepository.GetAll()
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.Name));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            return View(_subjectRepository.GetAll().OrderBy(s => s.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (_subjectRepository.SubjectExistsByCode(subject.Code))
                {
                    ModelState.AddModelError("Code", "This code is already in use.");
                    return View(subject);
                }

                subject.User = await _userHelper.GetUserByEmailAsync("school_manager@gmail.com");
                await _subjectRepository.CreateAsync(subject);
                return RedirectToAction(nameof(Manage));
            }
            return View(subject);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subject.User = await _userHelper.GetUserByEmailAsync("school_manager@gmail.com");
                    await _subjectRepository.UpdateAsync(subject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _subjectRepository.ExistAsync(subject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(subject);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id); ;
            await _subjectRepository.DeleteAsync(subject);
            return RedirectToAction(nameof(Manage));
        }
    }
}
