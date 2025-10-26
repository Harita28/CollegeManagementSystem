namespace CollegeManagementSystem.Models
{
    public class ProfessorSubject
    {
        public long Id { get; set; }
        public string ProfessorId { get; set; } = null!; // FK to AspNetUsers (Identity)
        public long SubjectId { get; set; }

        public ApplicationUser? Professor { get; set; }
        public Subject? Subject { get; set; }
    }
}
