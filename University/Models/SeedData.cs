﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using University.Data;

namespace University.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniversityContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UniversityContext>>()))
            {
                // Look for any movies.
                if (context.Course.Any() || context.Teacher.Any() || context.Student.Any())
                {
                    return;   // DB has been seeded
                }

                context.Teacher.AddRange(
                    new Teacher { /*Id = 1, */firstName = "Kim", lastName = "Abercrombie" },
                    new Teacher { /*Id = 2, */firstName = "Roger", lastName = "Hauri"},
                    new Teacher { /*Id = 3, */firstName = "Candace", lastName = "Kapoor"},
                    new Teacher { /*Id = 4, */firstName = "Roger", lastName = "Zheng" }
                );
                context.SaveChanges();

                context.Student.AddRange(
                    new Student { studentID = "1982017", firstName = "Carson", lastName = "Alexander", enrollmentDate = DateTime.Parse("2017-09-01") },
                    new Student { studentID = "1222017", firstName = "Meredith", lastName = "Alonso", enrollmentDate = DateTime.Parse("2017-09-01") },
                    new Student { studentID = "1892017", firstName = "Arturo", lastName = "Anand", enrollmentDate = DateTime.Parse("2017-09-01") },
                    new Student { studentID = "802017", firstName = "Gytis", lastName = "Barzdukas", enrollmentDate = DateTime.Parse("2017-09-01") },
                    new Student { studentID = "182018", firstName = "Yan", lastName = "Li", enrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { studentID = "12018", firstName = "Peggy", lastName = "Justice", enrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { studentID = "1902019", firstName = "Laura", lastName = "Norman", enrollmentDate = DateTime.Parse("2019-09-01") },
                    new Student { studentID = "3042017", firstName = "Nino", lastName = "Olivetto", enrollmentDate = DateTime.Parse("2017-09-01") }
                );
                context.SaveChanges();

                context.Course.AddRange(
                    new Course
                    {
                        ID = 1050,
                        title = "Chemistry",
                        credits = 6,
                        semester = 1,
                        firstTeacherID = context.Teacher.Single(d => d.firstName == "Kim" && d.lastName == "Abercrombie").ID,
                        secondTeacherID = context.Teacher.Single(d => d.firstName == "Roger" && d.lastName == "Zheng").ID
                    },

                    new Course
                    {
                        ID = 4022,
                        title = "Microeconomics",
                        credits = 3,
                        semester = 5,
                        firstTeacherID = context.Teacher.Single(d => d.firstName == "Kim" && d.lastName == "Abercrombie").ID,
                        secondTeacherID = context.Teacher.Single(d => d.firstName == "Candace" && d.lastName == "Kapoor").ID
                    },

                    new Course
                    {
                        ID = 4041,
                        title = "Macroeconomics",
                        credits = 3,
                        semester = 5,
                        firstTeacherID = context.Teacher.Single(d => d.firstName == "Roger" && d.lastName == "Zheng").ID,
                        secondTeacherID = context.Teacher.Single(d => d.firstName == "Roger" && d.lastName == "Hauri").ID
                    },

                    new Course
                    {
                        ID = 1045,
                        title = "Calculus",
                        credits = 6,
                        semester = 1,
                        firstTeacherID = context.Teacher.Single(d => d.firstName == "Kim" && d.lastName == "Abercrombie").ID,
                        secondTeacherID = context.Teacher.Single(d => d.firstName == "Roger" && d.lastName == "Zheng").ID
                    },

                    new Course
                    {
                        ID = 3141,
                        title = "Trigonometry",
                        credits = 3,
                        semester = 2,
                        firstTeacherID = context.Teacher.Single(d => d.firstName == "Kim" && d.lastName == "Abercrombie").ID,
                        secondTeacherID = context.Teacher.Single(d => d.firstName == "Candace" && d.lastName == "Kapoor").ID
                    },

                    new Course
                    {
                        ID = 2021,
                        title = "Composition",
                        credits = 6,
                        semester = 4,
                        firstTeacherID = context.Teacher.Single(d => d.firstName == "Candace" && d.lastName == "Kapoor").ID,
                        secondTeacherID = context.Teacher.Single(d => d.firstName == "Roger" && d.lastName == "Zheng").ID
                    },

                    new Course
                    {
                        ID = 2042,
                        title = "Literature",
                        credits = 6,
                        semester = 3,
                        firstTeacherID = context.Teacher.Single(d => d.firstName == "Kim" && d.lastName == "Abercrombie").ID,
                        secondTeacherID = context.Teacher.Single(d => d.firstName == "Roger" && d.lastName == "Hauri").ID
                    }
                );
                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment { studentID = 1, courseID = 1050 },
                    new Enrollment { studentID = 1, courseID = 4022 },
                    new Enrollment { studentID = 1, courseID = 4041 },
                    new Enrollment { studentID = 2, courseID = 1045 },
                    new Enrollment { studentID = 2, courseID = 3141 },
                    new Enrollment { studentID = 2, courseID = 2021 },
                    new Enrollment { studentID = 3, courseID = 1050 },
                    new Enrollment { studentID = 4, courseID = 1050 },
                    new Enrollment { studentID = 4, courseID = 4022 },
                    new Enrollment { studentID = 5, courseID = 4041 },
                    new Enrollment { studentID = 6, courseID = 1045 },
                    new Enrollment { studentID = 7, courseID = 3141 }
                );

                context.SaveChanges();
            }
        }
    }
}
