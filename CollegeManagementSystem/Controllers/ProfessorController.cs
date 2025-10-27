using CollegeManagementSystem.Data;
using CollegeManagementSystem.Models;
using CollegeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _users;
        private readonly IWebHostEnvironment _env;

        public ProfessorController(ApplicationDbContext db, UserManager<ApplicationUser> users, IWebHostEnvironment env)
        {
            _db = db;
            _users = users;
            _env = env;
        }

        public IActionResult Index() => View();

        public IActionResult AddAssignment()
        {
            var profId = _users.GetUserId(User);
            var subjects = _db.ProfessorSubjects
                .Where(ps => ps.ProfessorId == profId)
                .Select(ps => ps.Subject)
                .ToList();

            ViewBag.Subjects = subjects;
            return View(new AddAssignmentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(AddAssignmentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var profId = _users.GetUserId(User);
                ViewBag.Subjects = _db.ProfessorSubjects.Where(ps => ps.ProfessorId == profId).Select(ps => ps.Subject).ToList();
                return View(vm);
            }

            string? fileUrl = null;
            if (vm.Attachment != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "assignments");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fname = $"{Guid.NewGuid()}_{Path.GetFileName(vm.Attachment.FileName)}";
                var full = Path.Combine(uploads, fname);
                using (var fs = new FileStream(full, FileMode.Create))
                {
                    await vm.Attachment.CopyToAsync(fs);
                }
                fileUrl = $"/uploads/assignments/{fname}";
            }

            var assignment = new Assignment
            {
                Title = vm.Title,
                Description = vm.Description,
                SubjectId = vm.SubjectId,
                Year = vm.Year,
                Semester = vm.Semester,
                Branch = vm.Branch,
                DueDate = vm.DueDate,
                AttachmentUrl = fileUrl,
                CreatedById = _users.GetUserId(User)
            };

            _db.Assignments.Add(assignment);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
