using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace University.Migrations
{
    public partial class CourseStudentTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentID = table.Column<string>(maxLength: 10, nullable: false),
                    firstName = table.Column<string>(maxLength: 50, nullable: false),
                    lastName = table.Column<string>(maxLength: 50, nullable: false),
                    enrollmentDate = table.Column<DateTime>(nullable: false),
                    acquiredCredits = table.Column<int>(nullable: false),
                    currentSemestar = table.Column<int>(nullable: false),
                    educationLevel = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(maxLength: 50, nullable: false),
                    lastName = table.Column<string>(maxLength: 50, nullable: false),
                    degree = table.Column<string>(maxLength: 50, nullable: true),
                    academicRank = table.Column<string>(maxLength: 25, nullable: true),
                    officeNumber = table.Column<string>(maxLength: 10, nullable: true),
                    hireDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    title = table.Column<string>(maxLength: 100, nullable: false),
                    credits = table.Column<int>(nullable: false),
                    semester = table.Column<int>(nullable: false),
                    programme = table.Column<string>(maxLength: 100, nullable: true),
                    educationLevel = table.Column<string>(maxLength: 25, nullable: true),
                    firstTeacherID = table.Column<int>(nullable: true),
                    secondTeacherID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Course_Teacher_firstTeacherID",
                        column: x => x.firstTeacherID,
                        principalTable: "Teacher",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Course_Teacher_secondTeacherID",
                        column: x => x.secondTeacherID,
                        principalTable: "Teacher",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseID = table.Column<int>(nullable: false),
                    studentID = table.Column<long>(nullable: false),
                    semester = table.Column<string>(maxLength: 10, nullable: true),
                    year = table.Column<int>(nullable: false),
                    grade = table.Column<int>(nullable: false),
                    seminalURL = table.Column<string>(maxLength: 255, nullable: true),
                    projectURL = table.Column<string>(maxLength: 255, nullable: true),
                    examPoints = table.Column<int>(nullable: false),
                    seminalPoints = table.Column<int>(nullable: false),
                    projectPoints = table.Column<int>(nullable: false),
                    additionalPoints = table.Column<int>(nullable: false),
                    finnishDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_courseID",
                        column: x => x.courseID,
                        principalTable: "Course",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_studentID",
                        column: x => x.studentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_firstTeacherID",
                table: "Course",
                column: "firstTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_secondTeacherID",
                table: "Course",
                column: "secondTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_courseID",
                table: "Enrollment",
                column: "courseID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_studentID",
                table: "Enrollment",
                column: "studentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}
