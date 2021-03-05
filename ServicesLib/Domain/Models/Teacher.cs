using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesLib.Domain.Models
{
    public class Teacher
    {
        [Key]
        [Required(ErrorMessage = ("Teacher ID required"))]
        [Display(Name = "Teacher ID:")]
        [StringLength(7, ErrorMessage = "Maximum Length is 7")]
        [ConcurrencyCheck]
        public string TeacherID { get; set; }

        [Required(ErrorMessage = ("First Name Required"))]
        [Display(Name = "First Name:")]
        [StringLength(20, MinimumLength =2,ErrorMessage = "Minimum 2 and Maximum 20 characters")]
        public string FirstName { get; set; } = "";


        [Required(ErrorMessage = ("Surname Name Required"))]
        [Display(Name = "Surname:")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Minimum 2 and Maximum 20 characters")]
        public string SurName { get; set; } = "";

        [Required(ErrorMessage = ("Password is Required"))]
        [Display(Name = "Password:")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Minimum 8 and Maximum 20 Character")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = ("Address 1 is required"))]
        [Display(Name = "Address 1:")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum 3 and Maximum 30 characters")]
        public string AddressOne { get; set; } = "";

        [Required(ErrorMessage = ("Address 2 is required"))]
        [Display(Name = "Address 2:")]
        [StringLength(30,MinimumLength =3, ErrorMessage = "Minimum 3 and Maximum 30 characters")]

        public string AddressTwo { get; set; } = "";

        [Required(ErrorMessage = "Town is required.")]
        [Display(Name = "Town:")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum 3 and Maximum 30 characters")]
        public string Town { get; set; } = "";

        [Required(ErrorMessage = ("County is required"))]
        [Display(Name = "County:")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum 3 and Maximum 30 characters")]
        public string County { get; set; } = "";

        [Required(ErrorMessage = ("Mobile number is required."))]
        [Display(Name = "Mobile number:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(14, ErrorMessage = "Maximum 14 Characters")]
        [RegularExpression(@"[\+]{1}[1-9]{1,3}[0-9]{9}", ErrorMessage = "[+][country code][area code][number] 12-14 numeric characters; no spaces or hyphens")]
        public string MobilePhoneNumber { get; set; } = "";

        [Required(ErrorMessage = ("Email address is Required "))]
        [Display(Name = "Email:")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Invalid Email Address")]
        public string EmailAddress { get; set; } = "";

        [Required(ErrorMessage = "Emergency Number is Required.")]
        [Display(Name = "Emergency contact number:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(14, ErrorMessage = "Maximum 14 Characters")]
        [RegularExpression(@"[\+]{1}[1-9]{1,3}[0-9]{9}", ErrorMessage = "[+][country code][area code][number] 12-14 numeric characters; no spaces or hyphens")]
        public string EmergencyMobilePhoneNumber { get; set; } = "";

        // PPS is 8 numeric characters
        [Required(ErrorMessage = ("PPS Number is Required"))]
        [Display(Name = "Teacher PPS Number:")]
        [StringLength(10)]
        [RegularExpression(@"[0-9]{7}[A-Z]{1,2}", ErrorMessage = ("PPS is 7 Numeric and 1 or 2 Characters"))]
        public string TeacherPPS { get; set; } = "";


        [Required(ErrorMessage = ("Please Select a Gender"))]
        [Display(Name = "Gender:")]
        public string GenderType { get; set; }

        //Either Full time or Part Time  
        [Required(ErrorMessage = ("Please Select Part time or Full Time"))]
        [Display(Name = "FullTime / PartTime:")]
        public string FullOrPartTime { get; set; }

        public byte[] TeacherImage { get; set; }

        // Teacher programme of study - Foreign Key        
        [Display(Name = "Teacher Programme:")]
        [Required(ErrorMessage ="Please Select a Programme")]
        [ForeignKey("Programme")]
        public string ProgrammeID { get; set; } = "";
        public Programme Programme { get; set; }
    }
}
