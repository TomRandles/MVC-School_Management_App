using Microsoft.AspNetCore.Http;
using ServicesLib.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.ViewModel
{
    public partial class StudentRegisterViewModel
    {
        [Key]
        [Required(ErrorMessage = ("Student ID required"))]
        [Display(Name = "Student ID:")]
        [StringLength(7, ErrorMessage = "Must be a single \'S\' character followed by 6 digits")]
        [RegularExpression(@"[sS]{1}[0-9]{6}")]
        [ConcurrencyCheck]
        public string StudentID { get; set; }

        [Required(ErrorMessage = ("First name required"))]
        [Display(Name = "Student first name:")]
        [StringLength(20, ErrorMessage = "Max 20 characters")]
        [RegularExpression(@"[\w\'\-\s\,\.]{2,20}")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = ("Surname name required"))]
        [Display(Name = "Student surname:")]
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
        [Display(Name = "Student PPS number:")]
        [StringLength(10)]
        [RegularExpression(@"[0-9]{7}[A-Z]{1,2}")]
        public string StudentPPS { get; set; } = "";

        // 
        [Display(Name = "Programme fee paid:")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [RegularExpression(@"\d{0,5}\.{0,1}\d{0,2}")]
        //[StudentProgrammeFee]
        public decimal ProgrammeFeePaid { get; set; } = 0;

        [Required(ErrorMessage = ("Student date of birth incorrect. Format DD/MM/YYYY"))]
        [Display(Name = "Student date of birth:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        // StudentDateOfBirth - custom validation - check student's age is between 18 and 60
        //[StudentDateOfBirth]
        public DateTime DateOfBirth { get; set; }

        public  IFormFile Image { get; set; }

        [Required(ErrorMessage = ("Gender should be  \"male\" or \"female\""))]
        [Display(Name = "Gender:")]
        public string GenderType { get; set; }

        //Either Full time or Part Time  
        [Required(ErrorMessage = ("Enter either \"fulltime\" or \"parttime\""))]
        [Display(Name = "Fulltime / parttime:")]
        public string FullOrPartTime { get; set; } 

        public byte[] StudentImage { get; set; }

        // Student programme of study - Foreign Key        
        [Display(Name = "Student Programme ID:")]
        [StringLength(6)]
       // [RegularExpression(@"\w{6}")]
        [ForeignKey("Programme")]
        public string ProgrammeID { get; set; } = "";

        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }

        public string GetRandomID()
        {
            string studentID;
            Random r = new Random();

            studentID = 'S' + r.Next(100_000, 999_999).ToString();
            return studentID;
        }

        // Override ToString to provide the student name as a single string
        public override string ToString()
        {
            return $"{FirstName} {SurName}";
        }
    }
}
