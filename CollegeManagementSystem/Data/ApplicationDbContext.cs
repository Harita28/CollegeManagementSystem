using CollegeManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ProfessorSubject> ProfessorSubjects { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentStatus> AssignmentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enums mapping to strings
            builder.Entity<ApplicationUser>().Property(u => u.Role).HasConversion<string>();
            builder.Entity<ApplicationUser>().Property(u => u.Branch).HasConversion<string>();
            builder.Entity<Assignment>().Property(a => a.Branch).HasConversion<string>();
            builder.Entity<Assignment>().Property(a => a.Semester).HasConversion<string>();
            builder.Entity<Subject>().Property(s => s.Branch).HasConversion<string>();
            builder.Entity<Subject>().Property(s => s.Semester).HasConversion<string>();
            builder.Entity<AssignmentStatus>().Property(s => s.Status).HasConversion<string>();

            // Relations
            builder.Entity<ProfessorSubject>()
                .HasOne(ps => ps.Professor)
                .WithMany()
                .HasForeignKey(ps => ps.ProfessorId);

            builder.Entity<ProfessorSubject>()
                .HasOne(ps => ps.Subject)
                .WithMany()
                .HasForeignKey(ps => ps.SubjectId);

            builder.Entity<Assignment>()
                .HasOne(a => a.Subject)
                .WithMany()
                .HasForeignKey(a => a.SubjectId);

            builder.Entity<AssignmentStatus>()
                .HasOne(s => s.Assignment)
                .WithMany()
                .HasForeignKey(s => s.AssignmentId);

            builder.Entity<AssignmentStatus>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId);
        }
    }
}
