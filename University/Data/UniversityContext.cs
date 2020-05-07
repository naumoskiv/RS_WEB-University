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
            builder.Entity<Enrollment>()
                .HasOne<Student>(p => p.student)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(p => p.studentID);
            //.HasPrincipalKey(p => p.Id);

            builder.Entity<Enrollment>()
                .HasOne<Course>(p => p.course)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(p => p.courseID);
            //.HasPrincipalKey(p => p.Id);



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
