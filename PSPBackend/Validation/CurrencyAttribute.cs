using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class CurrencyAttibute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is string currency)
        {
            if(currency.Length!=3)
            {
                return new ValidationResult("Currency has to be made of 3 characters.");
            }
            if(!Regex.IsMatch(currency, @"^[a-zA-Z ]+$"))
            {
                return new ValidationResult("Currency has to consist of only letters.");
            }
        }
        return ValidationResult.Success;
    }
}