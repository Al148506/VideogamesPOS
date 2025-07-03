using System;
using System.ComponentModel.DataAnnotations;

public class MinimumAgeAttribute : ValidationAttribute
{
    private readonly int _minimumAge;

    public MinimumAgeAttribute(int minimumAge)
    {
        _minimumAge = minimumAge;
        ErrorMessage = $"You must be at least {_minimumAge} years old.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("Birth date is required.");

        if (value is DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age)) age--; // Ajuste si aún no ha cumplido los años este año

            if (age < _minimumAge)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid date format.");
    }
}
