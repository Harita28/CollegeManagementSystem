namespace CollegeManagementSystem.Models
{
    public class Subject
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public Branch Branch { get; set; }
        public Semester Semester { get; set; }
    }
}
