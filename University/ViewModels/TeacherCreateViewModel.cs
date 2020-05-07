using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using University.Models;

namespace University.ViewModels
{
    public class TeacherCreateViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Degree")]
        public string? degree { get; set; }

        [StringLength(25)]
        [Display(Name = "Academic Rank")]
        public string? academicRank { get; set; }

        [StringLength(10)]
        [Display(Name = "Office Number")]
        public string? officeNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")] 
        public DateTime? hireDate { get; set; }

        [Display(Name = "Picture")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Full Name")]
        public string fullName
        {
            get { return firstName + " " + lastName; }
        }

        [Display(Name = "First Courses")]
        public ICollection<Course> firstCourses { get; set; }

        [Display(Name = "Second Courses")]
        public ICollection<Course> secondCourses { get; set; }


    }
}
