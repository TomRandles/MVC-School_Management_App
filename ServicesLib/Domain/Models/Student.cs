using ServicesLib.Domain.Models.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesLib.Domain.Models
{
    public class Student
    {
        [Key]
        [Required(ErrorMessage = ("Student ID required"))]
        [Display(Name = "Student ID:")]
        [StringLength(50, ErrorMessage = "No more than 50 characters")]
        [RegularExpression(@"[sS]{1}[0-9]{6}")]
        [ConcurrencyCheck]
        public string StudentID { get; set; }

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
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum 3 and Maximum 30 characters")]

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
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; } = "";

        [Required(ErrorMessage = "Emergency Number is Required.")]
        [Display(Name = "Emergency contact number:")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(14, ErrorMessage = "Maximum 14 Characters")]
        [RegularExpression(@"[\+]{1}[1-9]{1,3}[0-9]{9}", ErrorMessage = "[+][country code][area code][number] 12-14 numeric characters; no spaces or hyphens")]
        public string EmergencyMobilePhoneNumber { get; set; } = "";

        // PPS is 8 numeric characters
        [Required(ErrorMessage = ("PPS Number is Required"))]
        [Display(Name = "Student PPS Number:")]
        [StringLength(10)]
        [RegularExpression(@"[0-9]{7}[A-Z]{1,2}", ErrorMessage = ("PPS is 7 Numeric and 1 or 2 Characters"))]
        public string StudentPPS { get; set; } = "";

        // 
        [Display(Name = "Programme Fee Paid:")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [RegularExpression(@"\d{0,5}\.{0,1}\d{0,2}",ErrorMessage ="Invalid Input")]
        [StudentProgrammeFee]
        public decimal ProgrammeFeePaid { get; set; } = 0;

        [Required(ErrorMessage = ("Student Date of Birth Incorrect. Format DD/MM/YYYY"))]
        [Display(Name = "Student Date of Birth:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        // StudentDateOfBirth - custom validation - check student's age is between 18 and 60
        [StudentDateOfBirth]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = ("Please Select a Gender"))]
        [Display(Name = "Gender:")]
        public string GenderType { get; set; }

        //Either Full time or Part Time  
        [Required(ErrorMessage = ("Please Select Part time or Full Time"))]
        [Display(Name = "FullTime / PartTime:")]
        public string FullOrPartTime { get; set; }

        public byte[] StudentImage { get; set; }

        // Student programme of study - Foreign Key        
        [Display(Name = "Student Programme:")]
        [StringLength(6)]
        [Required(ErrorMessage = "Please Select a Programme")]
        [ForeignKey("Programme")]
        public string ProgrammeID { get; set; } = "";
        public Programme Programme { get; set; }

        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }

        public string GetRandomID()
        {
            string studentID;
            Random r = new Random();

            studentID = 'S' + r.Next(100_000, 999_999).ToString();
            return studentID;
        }
    }
}
