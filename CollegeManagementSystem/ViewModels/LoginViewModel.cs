using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] [DataType(DataType.Password)] public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
