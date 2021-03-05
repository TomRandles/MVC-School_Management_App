using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesLib.Domain.Models
{
    public class Assessment
    {
        // Assessment ID - Primary Key
        [Key]
        [Required(ErrorMessage = ("Assessment ID required"))]
        [Display(Name = "Assessment ID")]
        [StringLength(6, ErrorMessage = "Maximum 6 characters")]
        [RegularExpression(@"\w{6}", ErrorMessage = "Must be 6 characters")]
        [ConcurrencyCheck]
        public string AssessmentID { get; set; }

        // Assessment name
        [Required]
        [Display(Name = "Assessment Name")]
        [StringLength(30, ErrorMessage = "No more than 30 characters")]
        public string AssessmentName { get; set; } = "";

        // Assessment description - optional
        [Display(Name = "Assessment Description")]
        [StringLength(100, ErrorMessage = "No more than 100 characters")]
        public string AssessmentDescription { get; set; } = "";

        // Total marks 
        [Required(ErrorMessage ="Assessment Maximum Mark is Required")]
        [Display(Name = "Assessment Maximum Mark")]
        [RegularExpression(@"\d{1,3}", ErrorMessage = "Must Be Number in Between 3 Digit Numbers")]
        public int AssessmentTotalMark { get; set; } = 0;

        public enum AssessmentTypeE
        {
            project,
            exam
        }

        // Assessment type
        [Required(ErrorMessage = ("Please Select a Type"))]
        [Display(Name = "Assessment type")]
        [EnumDataType(typeof(AssessmentTypeE))]
        public AssessmentTypeE AssessmentType { get; set; } = AssessmentTypeE.project;

        // Foreign key to Module
        [StringLength(6, ErrorMessage = "Must be 6 characters")]
        [Required(ErrorMessage ="Please Select a Module")]
        [RegularExpression(@"\w{6}", ErrorMessage = "Must be 6 characters")]
        [ForeignKey("Module")]
        public string ModuleID { get; set; } = "";

        public Module Module { get; set; }

        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }
    }
}
