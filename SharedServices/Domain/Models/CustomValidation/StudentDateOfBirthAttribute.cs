using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServicesLib.Domain.Models.CustomValidation
{
    public class StudentDateOfBirthAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object DateOfBirth, ValidationContext validationContext)
        {
            Student stud = (Student)validationContext.ObjectInstance;

            DateTime dateOfBirth = stud.DateOfBirth;

            // Calculate student's age in days. 
            TimeSpan ageInDays = DateTime.Now - dateOfBirth;

            // Student must be 18 or over and 60 or under
            if ((ageInDays.Days >= (365 * 18)) &&
                (ageInDays.Days <= (365 * 60))
               )
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Must be between 18 and 60 years of age");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-date-of-birth", "Must be between 18 and and 60 years");
        }
    }
}