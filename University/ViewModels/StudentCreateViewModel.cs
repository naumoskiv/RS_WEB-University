using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using University.Models;

namespace University.ViewModels
{
    public class StudentCreateViewModel
    {
        [Required]
        public Int64 ID { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Student ID")]
        public string studentID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime? enrollmentDate { get; set; }

        [Display(Name = "Acquired Credits")]
        public int? acquiredCredits { get; set; }

        [Display(Name = "Current Semester")]
        public int? currentSemestar { get; set; }

        [StringLength(25)]
        [Display(Name = "Education Level")]
        public string? educationLevel { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Full Name")]
        public string fullName
        {
            get { return firstName + " " + lastName; }
        }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
