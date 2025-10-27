using CollegeManagementSystem.Data;
using CollegeManagementSystem.Models;
using CollegeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CollegeManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly ApplicationDbContext _db;

        public AdminController(UserManager<ApplicationUser> users, ApplicationDbContext db)
        {
            _users = users;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProfessor()
        {
            // get branch from claim (safe outside LINQ)
            var branchClaim = User.FindFirst("Branch")?.Value;
            Branch branch = Branch.Computer;
            if (!string.IsNullOrEmpty(branchClaim) && Enum.TryParse(branchClaim, out Branch parsed)) branch = parsed;

            ViewBag.Subjects = _db.Subjects.Where(s => s.Branch == branch).ToList();
            return View(new AddProfessorViewModel { Branch = branch });
        }

        [HttpPost]
        public async Task<IActionResult> AddProfessor(AddProfessorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var branchClaim = User.FindFirst("Branch")?.Value;
                Branch branch = Branch.Computer;
                if (!string.IsNullOrEmpty(branchClaim) && Enum.TryParse(branchClaim, out Branch parsed)) branch = parsed;
                ViewBag.Subjects = _db.Subjects.Where(s => s.Branch == branch).ToList();
                return View(vm);
            }

            var user = new ApplicationUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                Name = vm.Name,
                Role = RoleEnum.Professor,
                Branch = vm.Branch,
                EmailConfirmed = true
            };

            var res = await _users.CreateAsync(user, vm.Password ?? "Prof@123");
            if (!res.Succeeded)
            {
                foreach (var e in res.Errors) ModelState.AddModelError("", e.Description);
                ViewBag.Subjects = _db.Subjects.Where(s => s.Branch == vm.Branch).ToList();
                return View(vm);
            }

            await _users.AddToRoleAsync(user, "Professor");
            await _users.AddClaimAsync(user, new Claim("Branch", vm.Branch.ToString()));

            if (vm.SubjectIds != null && vm.SubjectIds.Length > 0)
            {
                foreach (var sId in vm.SubjectIds)
                {
                    _db.ProfessorSubjects.Add(new ProfessorSubject
                    {
                        ProfessorId = user.Id,
                        SubjectId = sId
                    });
                }
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ViewProfessors()
        {
            var branchClaim = User.FindFirst("Branch")?.Value;
            Branch branch = Branch.Computer;
            if (!string.IsNullOrEmpty(branchClaim) && Enum.TryParse(branchClaim, out Branch parsed)) branch = parsed;

            var profIds = _db.Users.Where(u => u.Branch == branch && u.Role == RoleEnum.Professor).ToList();
            return View(profIds);
        }
    }
}
