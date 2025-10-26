namespace CollegeManagementSystem.Models
{
    public class Assignment
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public int Year { get; set; }
        public Semester Semester { get; set; }
        public Branch Branch { get; set; }
        public DateTime DueDate { get; set; }
        public string? AttachmentUrl { get; set; }
        public string CreatedById { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
