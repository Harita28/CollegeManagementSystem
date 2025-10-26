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

            // Create roles
            string[] roles = new[] { "SuperAdmin", "Admin", "Professor", "Student" };
            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }

            // Create superadmin
            var superEmail = "superadmin@college.local";
            var super = await userMgr.FindByEmailAsync(superEmail);
            if (super == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = superEmail,
                    Email = superEmail,
                    Name = "Super Admin",
                    Role = RoleEnum.SuperAdmin
                };
                var result = await userMgr.CreateAsync(adminUser, "Super@123"); // Change password later
                if (result.Succeeded)
                {
                    await userMgr.AddToRoleAsync(adminUser, "SuperAdmin");
                }
            }

            // Seed Subjects if none
            if (!db.Subjects.Any())
            {
                var branches = Enum.GetValues(typeof(Branch)).Cast<Branch>();
                var semesters = Enum.GetValues(typeof(Semester)).Cast<Semester>();

                foreach (var b in branches)
                {
                    foreach (var s in semesters)
                    {
                        // Add 3 sample subjects per sem for demo
                        for (int i = 1; i <= 3; i++)
                        {
                            db.Subjects.Add(new Subject
                            {
                                Name = $"{b} - Subj {s} - {i}",
                                Branch = b,
                                Semester = s
                            });
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
