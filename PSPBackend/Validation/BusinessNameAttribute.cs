using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class BusinessNameAttibute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is string name)
        {
            if(name.Length>255)
            {
                return new ValidationResult("Business name cannot exceed 255 characters.");
            }
            if(name.Length<3)
            {
                return new ValidationResult("Business name must be longer than 3 characters.");
            }
        }
        return ValidationResult.Success;
    }
}