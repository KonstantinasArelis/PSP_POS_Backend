using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ClientNameAttibute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is string name)
        {
            if(name.Length>255)
            {
                return new ValidationResult("Client name cannot exceed 255 characters.");
            }
            if(name.Length<3)
            {
                return new ValidationResult("Client name must be longer than 3 characters.");
            }
            if(!Regex.IsMatch(name, @"^[a-zA-Z ]+$"))
            {
                return new ValidationResult("Client name does is not valid");
            }
        }
        return ValidationResult.Success;
    }
}