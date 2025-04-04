using Microsoft.EntityFrameworkCore;

namespace WebAppEF.Models
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Student> Students { get; set; } = null;
        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string connectionString = Program.config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student(1, "Vasya", 18, "asddadad, adasdad,  asdasdad"),
                new Student(2, "Alex", 20, "asddadad, adasdad,  asdasdad"),
                new Student(3, "Coper", 30, "asddadad, adasdad,  asdasdad"),
                new Student(4, "Serega", 45, "asddadad, adasdad,  asdasdad"),
                new Student(5, "Endik", 19, "asddadad, adasdad,  asdasdad"));
        }
    }
}
