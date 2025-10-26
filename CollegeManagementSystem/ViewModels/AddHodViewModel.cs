using CollegeManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.ViewModels
{
    public class AddHodViewModel
    {
        [Required] public string Name { get; set; } = null!;
        [Required] [EmailAddress] public string Email { get; set; } = null!;
        [Required] public Branch Branch { get; set; }
        public string? Password { get; set; }
    }
}
