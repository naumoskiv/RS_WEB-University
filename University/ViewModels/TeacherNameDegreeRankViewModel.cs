using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.Models;

namespace University.ViewModels
{
    public class TeacherNameDegreeRankViewModel
    {
        public IList<Teacher> teachersVM { get; set; }
        public SelectList degreeVM { get; set; }
        public SelectList rankVM { get; set; }
        public string teacherDegree { get; set; }
        public string teacherRank { get; set; }
        public string searchString { get; set; }
    }
}
