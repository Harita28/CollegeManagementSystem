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
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Assignments()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var assignments = await _db.Assignments
                .Include(a => a.Subject)
                .Where(a => a.Branch == user.Branch && a.Semester == user.Semester && a.Year == (user.Year ?? a.Year))
                .ToListAsync();

            var statuses = await _db.AssignmentStatuses.Where(s => s.StudentId == userId).ToListAsync();

            ViewBag.Statuses = statuses;
            return View(assignments);
        }

        [HttpPost]
        public async Task<IActionResult> SetStatus(long assignmentId, AssignmentStatusEnum status)
        {
            var userId = _userManager.GetUserId(User);
            var existing = _db.AssignmentStatuses.FirstOrDefault(s => s.AssignmentId == assignmentId && s.StudentId == userId);
            if (existing == null)
            {
                existing = new AssignmentStatus { AssignmentId = assignmentId, StudentId = userId, Status = status };
                _db.AssignmentStatuses.Add(existing);
            }
            else
            {
                existing.Status = status;
                existing.UpdatedAt = DateTime.UtcNow;
                _db.AssignmentStatuses.Update(existing);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Assignments");
        }
    }
}
