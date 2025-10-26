using CollegeManagementSystem.Data;
using CollegeManagementSystem.Models;
using CollegeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddHod()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddHod(AddHodViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = new ApplicationUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                Name = vm.Name,
                Role = RoleEnum.Admin,
                Branch = vm.Branch,
                EmailConfirmed = true
            };
            var res = await _userManager.CreateAsync(user, vm.Password ?? "Hod@123");
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                // Send email functionality can be added here
                return RedirectToAction("Index", "SuperAdmin");
            }
            foreach (var err in res.Errors) ModelState.AddModelError("", err.Description);
            return View(vm);
        }

      [Authorize(Roles = "Admin")]
public IActionResult AddProfessor()
{
    var branchClaim = User.FindFirst("Branch")?.Value;
    Branch branchEnum = Branch.Computer; // default
    if (!string.IsNullOrEmpty(branchClaim) && Enum.TryParse(branchClaim, out Branch parsed))
        branchEnum = parsed;

    ViewBag.Subjects = _db.Subjects
        .Where(s => s.Branch == branchEnum)
        .ToList();

    return View();
}


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProfessor(AddProfessorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = new ApplicationUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                Name = vm.Name,
                Role = RoleEnum.Professor,
                Branch = vm.Branch,
                EmailConfirmed = true
            };
            var res = await _userManager.CreateAsync(user, vm.Password ?? "Prof@123");
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Professor");
                // assign subjects
                if (vm.SubjectIds != null)
                {
                    foreach (var sid in vm.SubjectIds)
                    {
                        _db.ProfessorSubjects.Add(new ProfessorSubject
                        {
                            ProfessorId = user.Id,
                            SubjectId = sid
                        });
                    }
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            foreach (var err in res.Errors) ModelState.AddModelError("", err.Description);
            return View(vm);
        }
    }
}
