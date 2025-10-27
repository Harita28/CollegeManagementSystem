using CollegeManagementSystem.Models;
using CollegeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CollegeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly UserManager<ApplicationUser> _users;

        public AccountController(SignInManager<ApplicationUser> signIn, UserManager<ApplicationUser> users)
        {
            _signIn = signIn;
            _users = users;
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _users.FindByEmailAsync(vm.Email);
            if (user == null) { ModelState.AddModelError("", "Invalid credentials"); return View(vm); }

            var res = await _signIn.PasswordSignInAsync(user, vm.Password, isPersistent: false, lockoutOnFailure: false);
            if (!res.Succeeded) { ModelState.AddModelError("", "Invalid credentials"); return View(vm); }

            // Determine role and redirect to the correct dashboard
            var roles = await _users.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            string? redirect = vm.ReturnUrl;
            if (string.IsNullOrEmpty(redirect))
            {
                redirect = role switch
                {
                    "SuperAdmin" => Url.Action("Index", "SuperAdmin"),
                    "Admin" => Url.Action("Index", "Admin"),
                    "Professor" => Url.Action("Index", "Professor"),
                    "Student" => Url.Action("Index", "Student"),
                    _ => Url.Action("Index", "Home")
                };
            }

            return Redirect(redirect ?? "/");
        }

        public IActionResult RegisterStudent() => View(new RegisterStudentViewModel());
        
        [HttpPost]
        public async Task<IActionResult> RegisterStudent(RegisterStudentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = new ApplicationUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                Name = vm.Name,
                Role = RoleEnum.Student,
                Branch = vm.Branch,
                Semester = vm.Semester,
                Year = vm.Year,
                EmailConfirmed = true
            };

            var create = await _users.CreateAsync(user, vm.Password);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors) ModelState.AddModelError("", e.Description);
                return View(vm);
            }

            await _users.AddToRoleAsync(user, "Student");
            // add Branch claim so controllers can filter
            await _users.AddClaimAsync(user, new Claim("Branch", vm.Branch.ToString()));

            await _signIn.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Student");
        }

        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
