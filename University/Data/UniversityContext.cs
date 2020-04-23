using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Models;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Course>()
                .HasOne(c => c.firstTeacher)
                .WithMany(t => t.firstCourses)
                .HasForeignKey(c => c.firstTeacherID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Course>()
                .HasOne(c => c.secondTeacher)
                .WithMany(t => t.secondCourses)
                .HasForeignKey(c => c.secondTeacherID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
