using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace University.Models
{
    public class Enrollment
    {
        public Int64 ID { get; set; }

        [Required]
        public int courseID { get; set; }

        [Required]
        public Int64 studentID { get; set; }

        [StringLength(10)]
        [Display(Name = "Semester")]
        public string? semester { get; set; }

        [Display(Name = "Year")]
        public int? year { get; set; }

        [Display(Name = "Grade")]
        public int? grade { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "Seminal URL")]
        public string? seminalURL { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "Project URL")]
        public string? projectURL { get; set; }
        
        [Display(Name = "Exam Points")]
        public int? examPoints { get; set; }
        
        [Display(Name = "Seminal Points")]
        public int? seminalPoints { get; set; }

        [Display(Name = "Project Points")]
        public int? projectPoints { get; set; }

        [Display(Name = "Additional Points")]
        public int? additionalPoints { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Finish Date")]
        public DateTime? finnishDate { get; set; }


        [Display(Name = "Student")]
        [ForeignKey("studentID")]
        public Student? student { get; set; }
        [Display(Name = "Course")]
        [ForeignKey("courseID")]
        public Course? course { get; set; }
    }
}
