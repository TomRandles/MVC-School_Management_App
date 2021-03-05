using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesLib.Domain.Models
{
    public class Programme
    {
        // Programme ID - Primary Key
        [Key]
        [Required(ErrorMessage = "Programme ID required")]
        [Display(Name = "Programme ID")]
        [StringLength(6, ErrorMessage = "Maximum 6 characters")]
        [RegularExpression(@"\w{6}",ErrorMessage ="Must Be 6 Characters")]
        [ConcurrencyCheck]
        public string ProgrammeID { get; set; }

        // Programme name
        [Required(ErrorMessage = "Programme Name required")]
        [Display(Name = "Programme Name")]
        [StringLength(20, MinimumLength =6,ErrorMessage = "6 to 20 characters")]

        public string ProgrammeName { get; set; } = "";

        // Programme description
        [Display(Name = "Programme Description")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters")]
        public string ProgrammeDescription { get; set; } = "";

        // QQI level
        [Display(Name = "Programme QQI Level")]
        [RegularExpression(@"\d{1}",ErrorMessage ="Must be only one Number")]
        public int ProgrammeQQILevel { get; set; } = 5;

        // Credits 
        [Display(Name = "Programme Credits")]
        [RegularExpression(@"\d{1,3}",ErrorMessage ="Must Be Number in Between 3 Digit Numbers")]
        public int ProgrammeCredits { get; set; } = 40;

        // Cost
        [Required(ErrorMessage = "Programme Cost Required")]
        [Display(Name = "Programme Cost")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [RegularExpression(@"\d{0,5}\.{0,1}\d{0,2}",ErrorMessage ="Invalid Programme Cost")]
        public decimal ProgrammeCost { get; set; } = 0;

        public virtual ICollection<Student> Students { get; set; }

    }
}
