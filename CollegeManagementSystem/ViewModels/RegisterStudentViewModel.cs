using CollegeManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.ViewModels
{
    public class RegisterStudentViewModel
    {
        [Required] public string Name { get; set; } = null!;
        [Required] [EmailAddress] public string Email { get; set; } = null!;
        [Required] [DataType(DataType.Password)] public string Password { get; set; } = null!;
        [Required] public Branch Branch { get; set; }
        [Required] public Semester Semester { get; set; }
        [Required] public int Year { get; set; }
    }
}
