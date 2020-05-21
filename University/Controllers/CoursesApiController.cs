using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;
using University.ViewModels;

namespace University.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesApiController : ControllerBase
    {
        private readonly UniversityContext _context;

        public CoursesApiController(UniversityContext context)
        {
            _context = context;
        }

        // GET: api/CoursesApi
        [HttpGet]
        public List<Course> GetCourse(int courseSemester,string courseProgramme, string searchString)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.title.Contains(searchString));
            }
            if (courseSemester > 0)
            {
                courses = courses.Where(x => x.semester == courseSemester);
            }
            if (!string.IsNullOrEmpty(courseProgramme))
            {
                courses = courses.Where(x => x.programme == courseProgramme);
            }
            return courses.ToList();
        }

        // GET: api/CoursesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/CoursesApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, EnrollmentsViewModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (id != model.course.ID) { return BadRequest(); }

            //Ne se sekjavam sto sum pravel...

            _context.Entry(model.course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                IEnumerable<long> listStudents = model.selectedStudents;
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
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CoursesApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourse", new { id = course.ID }, course);
        }

        // DELETE: api/CoursesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }


        // GET: api/CoursesApi
        [HttpGet("{id}/GetStudents")]
        public async Task<IActionResult> GetStudentsInCourse([FromRoute] int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var enrollments = _context.Enrollment.Where(e => e.courseID == id).ToList();
            List<Student> students = new List<Student>();
            foreach (var student in enrollments)
            {
                Student newstudent = _context.Student.Where(s => s.ID == student.ID).FirstOrDefault();
                newstudent.Enrollments = null;
                students.Add(newstudent);
            }
            return Ok(students);
        }



        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.ID == id);
        }
    }
}
