using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;
using University.ViewModels;

namespace University.Controllers
{
    public class TeachersController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IHostingEnvironment webHostEnvironment;

        public TeachersController(UniversityContext context, IHostingEnvironment hostingEnviroment)
        {
            _context = context;
            webHostEnvironment = hostingEnviroment;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string teacherD, string teacherR, string searchString)
        {
            IQueryable<Teacher> teachers = _context.Teacher.AsQueryable();
            IQueryable<string> degreeQuery = _context.Teacher.OrderBy(t => t.degree).Select(t => t.degree).Distinct();
            IQueryable<string> rankQuery = _context.Teacher.OrderBy(t => t.academicRank).Select(t => t.academicRank).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(t => t.firstName.Contains(searchString) || t.lastName.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(teacherD))
            {
                teachers = teachers.Where(x => x.degree == teacherD);
            }
            if (!string.IsNullOrEmpty(teacherR))
            {
                teachers = teachers.Where(x => x.academicRank == teacherR);
            }

            teachers = teachers.Include(t => t.firstCourses).Include(t => t.secondCourses);

            var teacherNameDegreeRankVM = new TeacherNameDegreeRankViewModel
            {
                degreeVM = new SelectList(await degreeQuery.ToListAsync()),
                rankVM = new SelectList(await rankQuery.ToListAsync()),
                teachersVM = await teachers.ToListAsync()
            };

            return View(teacherNameDegreeRankVM);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(t => t.firstCourses)
                .Include(t => t.secondCourses)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Teacher teacher = new Teacher
                {
                    ID = model.ID,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    degree = model.degree,
                    academicRank = model.academicRank,
                    officeNumber = model.officeNumber,
                    hireDate = model.hireDate,
                    profilePicture = uniqueFileName,
                };

                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = teacher.ID });
            }
            return View();
        }

        private string UploadedFile(TeacherCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl, [Bind("ID,firstName,lastName,degree,academicRank,officeNumber,hireDate,profilePicture")] Teacher teacher)
        {
            if (id != teacher.ID)
            {
                return NotFound();
            }

            TeachersController uploadImage = new TeachersController(_context, webHostEnvironment);
            teacher.profilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = teacher.ID });
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        //GET: Teachers/TeacherList
        public async Task<IActionResult> TeacherList()
        {
            var universityContext = _context.Teacher.Include(t => t.firstCourses).Include(t => t.secondCourses);
            return View(await universityContext.ToListAsync());
        }

        //GET: Teachers/TeacherViewDetails/5
        public async Task<IActionResult> TeacherViewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(t => t.firstCourses)
                .Include(t => t.secondCourses)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        //Overloaded function UploadedFile for Edit
        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.ID == id);
        }
    }
}
