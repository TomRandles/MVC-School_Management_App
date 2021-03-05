using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesLib.Domain.Models
{
    public class Module
    {
        // Module ID - Primary Key
        [Key]

        [Display(Name = "Module ID")]
        [StringLength(6, ErrorMessage = ("Maximum 6 characters"))]
        [RegularExpression(@"\w{6}", ErrorMessage = ("Invalid Input"))]
        [ConcurrencyCheck]
        public string ModuleID { get; set; }

        // Module name
        [Required]
        [Display(Name = "Module name")]
        [StringLength(30,MinimumLength =6,ErrorMessage = ("Minimum 6 and Maximum 30 characters"))]
        public string ModuleName { get; set; } = "";

        // Module description
        [Display(Name = "Module description")]
        [StringLength(50, ErrorMessage = ("Maximum 50 characters"))]
        public string ModuleDescription { get; set; } = "";

        // Credits 
        [Display(Name = "Module Credits")]
        [RegularExpression(@"\d{1,3}", ErrorMessage = "Must Be Number in Between 3 Digit Numbers")]
        public int ModuleCredits { get; set; } = 5;


        // Student programme of study - Foreign Key        
        [Display(Name = "Student Programme:")]
        [Required(ErrorMessage ="Please Select a Programme")]
        [StringLength(6)]
        [RegularExpression(@"\w{6}")]
        [ForeignKey("Programme")]
        public string ProgrammeID { get; set; } = "";
        public Programme Programme { get; set; }

        public virtual ICollection<Assessment> Assessments { get; set; }
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }


    }
}
