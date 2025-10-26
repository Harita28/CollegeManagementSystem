namespace CollegeManagementSystem.Models
{
    public enum RoleEnum
    {
        SuperAdmin,
        Admin,
        Professor,
        Student
    }

    public enum Branch
    {
        Computer,
        Mechanical,
        Electrical,
        Civil,
        Chemical
    }

    public enum Semester
    {
        SEM1 = 1,
        SEM2 = 2,
        SEM3 = 3,
        SEM4 = 4,
        SEM5 = 5,
        SEM6 = 6,
        SEM7 = 7,
        SEM8 = 8
    }

    public enum AssignmentStatusEnum
    {
        Pending,
        Completed,
        Done
    }
}
