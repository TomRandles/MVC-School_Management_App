using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesLib.Domain.Models
{
    public class ProgrammeModule
    {
        // The Complex primary key ProgrammeID-ModuleID is setup in CollegeDbContext 

        // Programme ID - Primary Key 1

        [ForeignKey("Programme")]
        [ConcurrencyCheck]
        [StringLength(6, ErrorMessage = "Must be 6 characters")]
        [RegularExpression(@"\w{6}")]
        public string ProgrammeID { get; set; }

        // Module ID - Primary Key 2
        [ForeignKey("Module")]
        [ConcurrencyCheck]
        [StringLength(6, ErrorMessage = "Must be 6 characters")]
        [RegularExpression(@"\w{6}")]
        public string ModuleID { get; set; }

        //public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }
    }
}
