using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.Models;


namespace University.ViewModels
{
    public class CourseTitleSemesterProgrammeViewModel
    {
        public IList<Course> coursesVM { get; set; }
        public SelectList semesterVM { get; set; }
        public SelectList programmeVM { get; set; }
        public int courseSemester { get; set; }
        public string courseProgramme { get; set; }
        public string searchString { get; set; }
    }
}
