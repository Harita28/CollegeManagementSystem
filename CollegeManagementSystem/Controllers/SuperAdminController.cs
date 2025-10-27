using CollegeManagementSystem.Data;
using CollegeManagementSystem.Models;
using CollegeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CollegeManagementSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly ApplicationDbContext _db;

        public SuperAdminController(UserManager<ApplicationUser> users, ApplicationDbContext db)
        {
            _users = users;
            _db = db;
        }

        public IActionResult Index() => View();

        public IActionResult AddHod() => View(new AddHodViewModel());

        [HttpPost]
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

            var res = await _users.CreateAsync(user, vm.Password ?? "Hod@123");
            if (!res.Succeeded)
            {
                foreach (var e in res.Errors) ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            await _users.AddToRoleAsync(user, "Admin");
            await _users.AddClaimAsync(user, new Claim("Branch", vm.Branch.ToString()));

            return RedirectToAction("Index");
        }

        public IActionResult ViewUsers()
        {
            var users = _db.Users.ToList();
            return View(users);
        }
    }
}
