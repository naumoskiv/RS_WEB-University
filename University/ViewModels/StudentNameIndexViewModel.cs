using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.Models;

namespace University.ViewModels
{
    public class StudentNameIndexViewModel
    {
        public IList<Student> studentVM { get; set; }
        public SelectList indexVM { get; set; }
        public string studentIndex { get; set; }
        public string searchString { get; set; }
    }
}
