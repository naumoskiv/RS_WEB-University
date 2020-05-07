using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace University.ViewModels
{
    public class EnrollmentsViewModel
    {
        public Course course { get; set; }
        public Enrollment enrollment { get; set; }
        public IEnumerable<long> selectedStudents { get; set; }
        public IEnumerable<SelectListItem> studentList { get; set; }

    }
}
