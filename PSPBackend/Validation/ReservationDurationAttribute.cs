using System.ComponentModel.DataAnnotations;

public class ReservationDurationAttribute : ValidationAttribute
{
    int maxReservationDurationMinutes = 24*60; // max duration is 1 day
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is int time && time >= 1 && time<maxReservationDurationMinutes){
            return new ValidationResult(ErrorMessage ?? "The reservation duration must be between 1 minute and 1 day in minutes");
        }
        
        return ValidationResult.Success;
    }
}