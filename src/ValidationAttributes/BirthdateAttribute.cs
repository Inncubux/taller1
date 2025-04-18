using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.ValidationAttributes
{
    public class BirthdateAttribute : ValidationAttribute
    {
        private readonly string _errorMessage =
        "La fecha de nacimiento debe ser menor o igual a la fecha actual.";

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value is not DateOnly birthdate)
                return new ValidationResult("El valor ingresado no es una fecha válida.");

            var timeZone = TimeZoneInfo.Local;
            var currentDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
            var currentDateOnly = DateOnly.FromDateTime(currentDate);

            if (currentDateOnly < birthdate)
                return new ValidationResult(_errorMessage);

            return ValidationResult.Success;
        }
    }
}