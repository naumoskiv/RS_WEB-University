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
using University.ViewModels;

namespace University.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IHostingEnvironment webHostingEnvironment;

        public StudentsController(UniversityContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentIndex, string searchString)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();
            IQueryable<string> indexQuery = _context.Student.OrderBy(s => s.studentID).Select(s => s.studentID).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(t => t.firstName.Contains(searchString) || t.lastName.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(studentIndex))
            {
                students = students.Where(x => x.studentID == studentIndex);
            }

            students = students.Include(s => s.Enrollments).ThenInclude(e => e.course);

            var studentNameIndexVM = new StudentNameIndexViewModel
            {
                indexVM = new SelectList(await indexQuery.ToListAsync()),
                studentVM = await students.ToListAsync()
            };

            return View(studentNameIndexVM);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Student student = new Student
                {
                    ID = model.ID,
                    studentID = model.studentID,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    enrollmentDate = model.enrollmentDate,
                    acquiredCredits = model.acquiredCredits,
                    currentSemestar = model.currentSemestar,
                    educationLevel = model.educationLevel,
                    profilePicture = uniqueFileName
                };

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { ID = student.ID });
            }
            return View();
        }

        private string UploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "studentImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, IFormFile imageUrl, StudentCreateViewModel model,[Bind("ID,studentID,firstName,lastName,enrollmentDate,acquiredCredits,currentSemestar,educationLevel,profilePicture")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            StudentsController uploadImage = new StudentsController(_context, webHostingEnvironment);
            student.profilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = student.ID });
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/StudentList
        public async Task<IActionResult> StudentList()
        {
            var universityContext = _context.Student.Include(s => s.Enrollments).ThenInclude(e => e.course);
            return View(await universityContext.ToListAsync());
        }


        // GET: Students/StudentViewDetails/5
        public async Task<IActionResult> StudentViewDetails(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        
        //Oveloaded function UploadedFile for Edit
        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "studentImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }



        private bool StudentExists(long id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
