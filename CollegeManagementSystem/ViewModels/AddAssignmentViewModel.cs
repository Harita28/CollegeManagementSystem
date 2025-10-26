using CollegeManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.ViewModels
{
    public class AddAssignmentViewModel
    {
        [Required] public string Title { get; set; } = null!;
        [Required] public string Description { get; set; } = null!;
        [Required] public long SubjectId { get; set; }
        [Required] public int Year { get; set; }
        [Required] public Semester Semester { get; set; }
        [Required] public Branch Branch { get; set; }
        [Required] public DateTime DueDate { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
