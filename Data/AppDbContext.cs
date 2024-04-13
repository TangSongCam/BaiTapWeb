using BaiTapWeb.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BaiTapWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Students> Students { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public IEnumerable<object> Image { get; internal set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            new Initial(builder).Seed();
        }
    }
}
