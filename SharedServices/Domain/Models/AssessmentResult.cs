using ServicesLib.Domain.Models.CustomValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesLib.Domain.Models
{
    public class AssessmentResult
    {
        // Assessment Result ID - Primary Key
        [Key]
        [StringLength(50, ErrorMessage = "No more than 50 characters")]
        [Display(Name = "Assessment Result ID")]
        [RegularExpression(@"[\-\w\.]{10,50}")]
        [ConcurrencyCheck]
        public string AssessmentResultID { get; set; }

        // Assessment result description - optional
        [Display(Name = "Assessment Feedback")]
        [StringLength(100, ErrorMessage = "No more than 100 characters")]

        public string AssessmentResultDescription { get; set; } = "";

        // Result mark 
        [Display(Name = "Assessment Result")]
        [Required(ErrorMessage ="Assessment Result Mark is Required")]
        [RegularExpression(@"^\d{1,3}\.{0,1}\d{0,2}$", ErrorMessage = "Mark must be in between 0 to 100")]
        [AssessmentResultMark]
        [Range(0, 100,ErrorMessage ="Mark must be in between 0 to 100")]
        public double AssessmentResultMark { get; set; } = 0;

        //Foreign Key - Student
        [Required(ErrorMessage ="Student Id is Required")]
        [Display(Name = "Student ID")]
        // [MaxLength(7)]
        [StringLength(50, ErrorMessage = "No more than 50 characters")]
        [RegularExpression(@"[sS]{1}[0-9]{6}")]
        [ForeignKey("Student")]
        public string StudentID { get; set; }
        public Student Student { get; set; }

        // Foreign Key - Programme ID
        [Required]
        [Display(Name = "Programme ID")]
        [MaxLength(6)]
        [RegularExpression(@"\w{6}")]
        [ForeignKey("ProgrammeModule")]
        public string ProgrammeID { get; set; }
        public Programme Programme { get; set; }

        [Display(Name = "Assessment Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = ("Assessment date required: format {0:dd-MM-yyyy}"))]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        //AssessmentDate - custom validation - check that date is within 6 months and not in the future
        [AssessmentDate]
        public DateTime AssessmentDate { get; set; }

        // Foreign Key - Module ID
        [Required]
        [Display(Name = "Module")]
        [StringLength(6, ErrorMessage = "Must be 6 characters")]
        [RegularExpression(@"\w{6}")]
        [ForeignKey("Module")]
        public string ModuleID { get; set; }
        public Module Module { get; set; }

        // Foreign Key - Assessment ID 
        [Required]
        [Display(Name = "Assessment")]
        [StringLength(6, ErrorMessage = "Must be 6 characters")]
        [RegularExpression(@"\w{6}")]
        [ForeignKey("Assessment")]
        public string AssessmentID { get; set; }
        public Assessment Assessment { get; set; }
    }
}
