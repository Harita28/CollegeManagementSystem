namespace CollegeManagementSystem.Models
{
    public class ProfessorSubject
    {
        public long Id { get; set; }
        public string ProfessorId { get; set; } = null!;
        public long SubjectId { get; set; }

        public ApplicationUser? Professor { get; set; }
        public Subject? Subject { get; set; }
    }
}
