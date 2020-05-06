using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;
using University.ViewModels;

namespace University.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UniversityContext _context;

        public CoursesController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int courseS, string courseP, string searchString)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> semesterQuery = _context.Course.OrderBy(c => c.semester).Select(c => c.semester).Distinct();
            IQueryable<string> programmeQuery = _context.Course.OrderBy(c => c.programme).Select(c => c.programme).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.title.Contains(searchString));
            }
            if (courseS > 0)
            {
                courses = courses.Where(x => x.semester == courseS);
            }
            if (!string.IsNullOrEmpty(courseP))
            {
                courses = courses.Where(x => x.programme == courseP);
            }

            courses = courses.Include(c => c.firstTeacher).Include(c => c.secondTeacher).Include(c => c.Enrollments)
                .ThenInclude(e => e.student);

            var courseTitleSemesterProgrammeVM = new CourseTitleSemesterProgrammeViewModel
            {
                semesterVM = new SelectList(await semesterQuery.ToListAsync()),
                programmeVM = new SelectList(await programmeQuery.ToListAsync()),
                coursesVM = await courses.ToListAsync()
            };

            var universityContext = _context.Course.Include(c => c.firstTeacher).Include(c => c.secondTeacher)
                .Include(c => c.Enrollments).ThenInclude(e => e.student);
            
            return View(courseTitleSemesterProgrammeVM);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.firstTeacher)
                .Include(c => c.secondTeacher)
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.student)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["firstTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName");
            ViewData["secondTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,title,credits,semester,programme,educationLevel,firstTeacherID,secondTeacherID")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["firstTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName", course.firstTeacherID);
            ViewData["secondTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName", course.secondTeacherID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Course.Where(c => c.ID == id).Include(c => c.Enrollments).First();

            if (course == null)
            {
                return NotFound();
            }

            EnrollmentsViewModel viewmodel = new EnrollmentsViewModel
            {
                course = course,
                studentList = new MultiSelectList(_context.Student.OrderBy(s => s.lastName), "ID", "fullName"),
                selectedStudents = course.Enrollments.Select(ss => ss.studentID)
            };


            ViewData["firstTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName", course.firstTeacherID);
            ViewData["secondTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName", course.secondTeacherID);
            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnrollmentsViewModel viewmodel)
        {
            if (id != viewmodel.course.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.course);
                    await _context.SaveChangesAsync();

                    IEnumerable<long> listStudents = viewmodel.selectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !listStudents.Contains(s.studentID) && s.courseID == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);

                    IEnumerable<long> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.studentID) && s.courseID == id).Select(s => s.studentID);
                    IEnumerable<long> newStudents = listStudents.Where(s => !existStudents.Contains(s));
                    foreach (int studentId in newStudents)
                        _context.Enrollment.Add(new Enrollment { studentID = studentId, courseID = id });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewmodel.course.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["firstTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName", viewmodel.course.firstTeacherID);
            ViewData["secondTeacherID"] = new SelectList(_context.Teacher, "ID", "fullName", viewmodel.course.secondTeacherID);
            return View(viewmodel);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.firstTeacher)
                .Include(c => c.secondTeacher)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CourseList()
        {
            var universityContext = _context.Course.Include(c => c.firstTeacher).Include(c => c.secondTeacher)
                .Include(c => c.Enrollments).ThenInclude(e => e.student);
            return View(await universityContext.ToListAsync());
        }

        public async Task<IActionResult> TeacherCourseView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.firstTeacher)
                .Include(c => c.secondTeacher)
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.student)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.ID == id);
        }
    }
}
