using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.Models;

namespace University.ViewModels
{
    public class YearFilterViewModel
    {
        public Course course { get; set; }
        
        [Display(Name = "Enrollment")]
        public IList<Enrollment> enrollmentVM { get; set; }
        public SelectList yearVM { get; set; }
        public int? enrollmentYear { get; set; }
    }
}
