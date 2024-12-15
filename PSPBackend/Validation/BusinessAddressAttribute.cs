using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class BusinessAddressAttibute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is string address)
        {
            if(address.Length>255)
            {
                return new ValidationResult("Business address cannot exceed 255 characters.");
            }
            if(address.Length<3)
            {
                return new ValidationResult("Business address must be longer than 3 characters.");
            }
        }
        return ValidationResult.Success;
    }
}