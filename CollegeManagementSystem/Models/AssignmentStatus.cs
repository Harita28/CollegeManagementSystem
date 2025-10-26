using System;
namespace CollegeManagementSystem.Models
{
    public class AssignmentStatus
    {
        public long Id { get; set; }
        public string StudentId { get; set; } = null!;
        public long AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public ApplicationUser? Student { get; set; }   // ✅ Add this line
        public AssignmentStatusEnum Status { get; set; } = AssignmentStatusEnum.Pending;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
