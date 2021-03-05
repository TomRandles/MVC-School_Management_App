using ServicesLib.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.ViewModel
{
    public class TeacherRegisterViewModel
    {
        [Key]
        [Required(ErrorMessage = ("Teacher ID required"))]
        [Display(Name = "Teacher ID:")]
        [StringLength(7, ErrorMessage = "Must be a single \'S\' character followed by 6 digits")]
        [RegularExpression(@"[sS]{1}[0-9]{6}")]
        [ConcurrencyCheck]
        public string TeacherID { get; set; }

        [Required(ErrorMessage = ("First name required"))]
        [Display(Name = "Teacher first name:")]
        [StringLength(20, ErrorMessage = "Max 20 characters")]
        [RegularExpression(@"[\w\'\-\s\,\.]{2,20}")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = ("Surname name required"))]
        [Display(Name = "Teacher surname:")]
        [StringLength(20, ErrorMessage = "Max 20 characters")]
        [RegularExpression(@"[\w\'\-\s\,\.]{2,20}")]
        public string SurName { get; set; } = "";

        [Required(ErrorMessage = ("Password name required"))]
        [Display(Name = "Password:")]
        [StringLength(20, ErrorMessage = "Min 8, max 20 characters")]
        [RegularExpression(@"[\w\d\$\&\-£]{8,20}")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = ("Address 1 is required"))]
        [Display(Name = "Address 1:")]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        [RegularExpression(@"[\s\w\-\,\.]{3,30}")]
        public string AddressOne { get; set; } = "";

        [Required(ErrorMessage = ("Address 2 is required"))]
        [Display(Name = "Address 2:")]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        [RegularExpression(@"[\s\w\-\,\.]{3,30}")]
        public string AddressTwo { get; set; } = "";

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Town:")]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        [RegularExpression(@"[\s\w\-\,\.\']{3,30}")]
        public string Town { get; set; } = "";

        [Required(ErrorMessage = ("County is required"))]
        [Display(Name = "County:")]
        [StringLength(30, ErrorMessage = "Max 30 characters")]
        [RegularExpression(@"[\s\w\-\,\.]{4,30}")]
        public string County { get; set; } = "";

        [Required(ErrorMessage = ("Mobile number is required."))]
        [Display(Name = "Mobile number:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(14, ErrorMessage = "[+][country code][area code][number] 12-14 numeric characters; no spaces or hyphens")]
        [RegularExpression(@"[\+]{1}[1-9]{1,3}[0-9]{9}")]
        public string MobilePhoneNumber { get; set; } = "";

        [Required(ErrorMessage = ("Email address is name@emailaddress.com "))]
        [Display(Name = "Email:")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = "";

        [Required(ErrorMessage = "Emergency number is required.")]
        [Display(Name = "Emergency contact number:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(14, ErrorMessage = "[+][country code][area code][number] 12-14 numeric characters; no spaces or hyphens")]
        [RegularExpression(@"[\+]{1}[1-9]{1,3}[0-9]{9}")]

        public string EmergencyMobilePhoneNumber { get; set; } = "";

        // PPS is 8 numeric characters
        [Required(ErrorMessage = ("PPS is 7 numbers and 1 or 2 characters"))]
        [Display(Name = "Teacher PPS Number:")]
        [StringLength(10)]
        [RegularExpression(@"[0-9]{7}[A-Z]{1,2}", ErrorMessage = "Invalid PPS Number")]
        public string TeacherPPS { get; set; } = "";

        // 
        [Display(Name = "Programme fee paid:")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [RegularExpression(@"\d{0,5}\.{0,1}\d{0,2}", ErrorMessage = "Invalid Programme Fee")]
        public decimal ProgrammeFeePaid { get; set; } = 0;


        [Required(ErrorMessage = ("Please Select a Gender"))]
        [Display(Name = "Gender:")]
        public string GenderType { get; set; }

        //Either Full time or Part Time  
        [Required(ErrorMessage = ("Please Select Part time or Full Time"))]
        [Display(Name = "FullTime / PartTime:")]
        public string FullOrPartTime { get; set; }

        public byte[] TeacherImage { get; set; }

        // Teacher programme of study - Foreign Key        
        [Display(Name = "Teacher Programme ID:")]
        [StringLength(6)]
        [RegularExpression(@"\w{6}")]
        [ForeignKey("Programme")]
        public string ProgrammeID { get; set; } = "";
        public Programme Programme { get; set; }
    }
}
