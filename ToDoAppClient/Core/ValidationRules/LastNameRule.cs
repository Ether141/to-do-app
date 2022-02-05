using System;
using System.Threading;
using System.Globalization;
using System.Windows.Controls;
using ToDoAppClient.Resources.Strings;

namespace ToDoAppClient.Core.ValidationRules
{
    public class LastNameRule : ValidationRule
    {
        public const int MinLength = 2;
        public const int MaxLength = 32;

        public ValidationResult Validate(object value) => Validate(value, Thread.CurrentThread.CurrentCulture);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == null)
                throw new ArgumentException("value argument is not string");

            string lastName = (string)value;

            if (string.IsNullOrEmpty(lastName))
                return new ValidationResult(false, Resource.giveLastName);

            if (lastName.Length < MinLength)
                return new ValidationResult(false, Resource.lastNameIsTooShort);

            if (lastName.Length > MaxLength)
                return new ValidationResult(false, Resource.lastNameIsTooLong);

            return ValidationResult.ValidResult;
        }
    }
}
