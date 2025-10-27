using CollegeManagementSystem.Data;
using CollegeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _users;

        public StudentController(ApplicationDbContext db, UserManager<ApplicationUser> users)
        {
            _db = db;
            _users = users;
        }

        public async Task<IActionResult> Index() => View();

        public async Task<IActionResult> Assignments()
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var assignments = await _db.Assignments.Include(a => a.Subject)
                .Where(a => a.Branch == user.Branch && a.Semester == user.Semester && a.Year == (user.Year ?? a.Year))
                .ToListAsync();

            var statuses = await _db.AssignmentStatuses.Where(s => s.StudentId == user.Id).ToListAsync();
            ViewBag.Statuses = statuses;
            return View(assignments);
        }

        [HttpPost]
        public async Task<IActionResult> SetStatus(long assignmentId, AssignmentStatusEnum status)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var existing = await _db.AssignmentStatuses.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == user.Id);
            if (existing == null)
            {
                _db.AssignmentStatuses.Add(new AssignmentStatus { AssignmentId = assignmentId, StudentId = user.Id, Status = status });
            }
            else
            {
                existing.Status = status;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Assignments");
        }
    }
}
