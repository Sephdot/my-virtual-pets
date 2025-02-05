using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace my_virtual_pets_class_library.DTO.Validators;

public class PasswordValidator : ValidationAttribute
{
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string Password)
            {
                string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
                if (Regex.IsMatch(Password, pattern))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("Min. 8 characters & 1 number");
        }

    
}