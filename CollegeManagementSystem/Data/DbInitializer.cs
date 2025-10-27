using CollegeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            // Roles
            string[] roles = new[] { "SuperAdmin", "Admin", "Professor", "Student" };
            foreach (var r in roles)
            {
                if (!await roleMgr.RoleExistsAsync(r))
                    await roleMgr.CreateAsync(new IdentityRole(r));
            }

            // Desired seeded SuperAdmin credentials
            var superEmail = "superadmin@college.login";
            var superPassword = "123";

            var existing = await userMgr.FindByEmailAsync(superEmail);
            if (existing == null)
            {
                var super = new ApplicationUser
                {
                    UserName = superEmail,
                    Email = superEmail,
                    Name = "Super Admin",
                    Role = RoleEnum.SuperAdmin,
                    EmailConfirmed = true
                };

                var create = await userMgr.CreateAsync(super, superPassword);
                if (create.Succeeded)
                {
                    await userMgr.AddToRoleAsync(super, "SuperAdmin");
                }
                else
                {
                    // If it fails (rare), try to surface first error to console
                    var firstError = create.Errors.FirstOrDefault()?.Description ?? "Unknown error creating SuperAdmin";
                    Console.WriteLine("Error creating SuperAdmin: " + firstError);
                }
            }
            else
            {
                // ensure role
                if (!await userMgr.IsInRoleAsync(existing, "SuperAdmin"))
                    await userMgr.AddToRoleAsync(existing, "SuperAdmin");

                // Reset the password to the requested one
                // To reset, remove the existing password (if any) and add a new one via token flow
                var token = await userMgr.GeneratePasswordResetTokenAsync(existing);
                var resetRes = await userMgr.ResetPasswordAsync(existing, token, superPassword);
                if (!resetRes.Succeeded)
                {
                    Console.WriteLine("Failed to reset SuperAdmin password. Errors:");
                    foreach (var e in resetRes.Errors) Console.WriteLine(e.Description);
                }
            }

            // seed example subjects if empty
            if (!db.Subjects.Any())
            {
                db.Subjects.AddRange(new[]
                {
                    new Subject { Name = "Programming Fundamentals", Branch = Branch.Computer, Semester = Semester.SEM1 },
                    new Subject { Name = "Engineering Mathematics I", Branch = Branch.Computer, Semester = Semester.SEM1 },
                    new Subject { Name = "Data Structures", Branch = Branch.Computer, Semester = Semester.SEM3 }
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
