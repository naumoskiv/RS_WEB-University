using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace University.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Required]
        [Display(Name = "Credits")]
        public int credits { get; set; }

        [Required]
        [Display(Name = "Semester")]
        public int semester { get; set; }

        [StringLength(100)]
        [Display(Name = "Programme")]
        public string? programme { get; set; }

        [StringLength(25)]
        [Display(Name = "Education Level")]
        public string? educationLevel { get; set; }

        [Display(Name = "First Teacher")]
        public int? firstTeacherID { get; set; }

        [Display(Name = "Second Teacher")]
        public int? secondTeacherID { get; set; }

        [Display(Name = "First Teacher")]
        [ForeignKey("firstTeacherID")]
        public Teacher? firstTeacher { get; set; }

        [Display(Name = "Second Teacher")]
        [ForeignKey("secondTeacherID")]
        public Teacher? secondTeacher { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
