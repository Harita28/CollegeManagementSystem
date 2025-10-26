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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProfessorController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
        }

        public IActionResult AddAssignment()
        {
            ViewBag.Subjects = _db.ProfessorSubjects
                .Where(ps => ps.ProfessorId == _userManager.GetUserId(User))
                .Select(ps => ps.Subject)
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(AddAssignmentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            string? filePath = null;
            if (vm.Attachment != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "assignments");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var filename = $"{Guid.NewGuid()}_{vm.Attachment.FileName}";
                var full = Path.Combine(uploads, filename);
                using (var fs = new FileStream(full, FileMode.Create))
                {
                    await vm.Attachment.CopyToAsync(fs);
                }
                filePath = $"/uploads/assignments/{filename}";
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
                AttachmentUrl = filePath,
                CreatedById = _userManager.GetUserId(User)
            };

            _db.Assignments.Add(assignment);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
