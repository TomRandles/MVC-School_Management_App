using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ServicesLib.Services.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServicesLib.Domain.Models.CustomValidation
{
    public class StudentProgrammeFeeAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object AssessmentResult, ValidationContext validationContext)
        {
            // Get current Assessment Result object 
            Student student = (Student)validationContext.ObjectInstance;

            // Check if student and student.ProgrammeID is set - may not always be. If not, exit without validation
             if ((student == null ) || (student.ProgrammeID == null))
             {
                  // Cannot complete validation - exit
                  return new ValidationResult("Student object not populated properly. Cannot complete validation.");
             }

            // Get DB context 
            var _db = (SchoolDbContext)validationContext
                         .GetService(typeof(SchoolDbContext));

            // Get the Programmes for this college
            IList<Programme> programmes = _db.Programmes.AsNoTracking().ToList();

            Programme programme = programmes.Where(p => p.ProgrammeID == student.ProgrammeID)
                                            .Select(p => p).FirstOrDefault();

            if (programme == null)
            {
                // Cannot complete validation - exit
                return new ValidationResult("Student object not populated properly. Cannot complete validation.");
            }

            // Check that the assessment result mark is less than the assessment total mark
            if (student.ProgrammeFeePaid <= programme.ProgrammeCost )
            {
                return ValidationResult.Success;
            }
            string valReturn = $"Student programme fee: {student.ProgrammeFeePaid} exceeds the programme cost: {programme.ProgrammeCost} ";
            return new ValidationResult(valReturn);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-student-programme-fee", "Student programme fee exceeds the programme cost");
        }
    }
}