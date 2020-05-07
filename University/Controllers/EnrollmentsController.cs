using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;

namespace University.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IHostingEnvironment webHostingEnvironment;

        public EnrollmentsController(UniversityContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var universityContext = _context.Enrollment.Include(e => e.course).Include(e => e.student);
            return View(await universityContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.course)
                .Include(e => e.student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title");
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,courseID,studentID,semester,year,grade,seminalURL,projectURL,examPoints,seminalPoints,projectPoints,additionalPoints,finnishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,courseID,studentID,semester,year,grade,seminalURL,projectURL,examPoints,seminalPoints,projectPoints,additionalPoints,finnishDate")] Enrollment enrollment)
        {
            if (id != enrollment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.ID))
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
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.course)
                .Include(e => e.student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Enrollents/CreateEnrollment
        public IActionResult CreateEnrollment()
        {
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title");
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName");
            return View();
        }

        //POST: Enrollments/CreateEnrollment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnrollment([Bind("ID,courseID,studentID,semester,year")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EnrollmentList));
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // GET: Enrollments/FinishEnrollment/5
        public async Task<IActionResult> FinishEnrollment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // POST: Enrollments/FinishEnrollment/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishEnrollment(long id, [Bind("ID,courseID,studentID,semester,year,grade,seminalURL,projectURL,examPoints,seminalPoints,projectPoints,additionalPoints,finnishDate")] Enrollment enrollment)
        {
            if (id != enrollment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(EnrollmentList));
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        public async Task<IActionResult> EnrollmentList()
        {
            var universityContext = _context.Enrollment.Include(e => e.course).Include(e => e.student);
            return View(await universityContext.ToListAsync());
        }



        // GET: Enrollments/TeacherEdit/5
        public async Task<IActionResult> TeacherEdit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // POST: Enrollments/TeacherEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherEdit(long id, [Bind("ID,courseID,studentID,semester,year,grade,seminalURL,projectURL,examPoints,seminalPoints,projectPoints,additionalPoints,finnishDate")] Enrollment enrollment)
        {
            if (id != enrollment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("TeacherList", "Teachers");
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // GET: Enrollments/StudentEdit/5
        public async Task<IActionResult> StudentEdit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }

        // POST: Enrollments/StudentEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEdit(long id, IFormFile pUrl, [Bind("ID,courseID,studentID,semester,year,grade,seminalURL,projectURL,examPoints,seminalPoints,projectPoints,additionalPoints,finnishDate")] Enrollment enrollment)
        {
            if (id != enrollment.ID)
            {
                return NotFound();
            }

            EnrollmentsController uploadUrl = new EnrollmentsController(_context, webHostingEnvironment);
            enrollment.seminalURL = uploadUrl.UploadedFile(pUrl);


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("StudentList", "Students");
            }
            ViewData["courseID"] = new SelectList(_context.Course, "ID", "title", enrollment.courseID);
            ViewData["studentID"] = new SelectList(_context.Student, "ID", "fullName", enrollment.studentID);
            return View(enrollment);
        }


        // GET: Enrollments/StudentViewEnrollment/5
        public async Task<IActionResult> StudentViewEnrollment(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.course)
                .Include(e => e.student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "projects");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            uniqueFileName = "/projects/" + uniqueFileName;
            return uniqueFileName;
        }



        private bool EnrollmentExists(long id)
        {
            return _context.Enrollment.Any(e => e.ID == id);
        }
    }
}
