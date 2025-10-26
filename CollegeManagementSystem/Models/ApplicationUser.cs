using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public RoleEnum Role { get; set; }
        public Branch? Branch { get; set; }
        public int? Year { get; set; }
        public Semester? Semester { get; set; }
        public string? MobileNo { get; set; }
    }
}
