using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace my_virtual_pets_class_library.DTO.Validators;




public class EmailValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string Email)
        {
            if (isValidEmail(Email))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult("This is not a valid email");
    }
    
    public bool isValidEmail(string email) {
        if (string.IsNullOrWhiteSpace(email)) {
            return false;
        }

        try {
            var addr = new MailAddress(email);
            return true;
        }
        catch {
            return false;
        }
    }

    
}