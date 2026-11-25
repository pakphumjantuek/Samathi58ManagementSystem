

namespace ProjectBlazorManage1.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Initial> Initials { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<EducationHistory>  EducationHistories { get; set; }
        public DbSet<CourseName> CourseNames { get; set; }
        public DbSet<DhammaCourse> DhammaCourses { get; set; }
        public DbSet<LearningPlan> LearningPlans { get; set; }
        public DbSet<ClassStudied> ClassStudieds { get; set; }
        public DbSet<Students> students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Personnel> Personnel { get; set; }

        // The connection string comes from configuration (ConnectionStrings:DefaultConnection),
        // registered in Program.cs. Never hardcode credentials here.

    }
}



