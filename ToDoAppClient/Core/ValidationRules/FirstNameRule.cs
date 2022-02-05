using System;
using System.Threading;
using System.Globalization;
using System.Windows.Controls;
using ToDoAppClient.Resources.Strings;

namespace ToDoAppClient.Core.ValidationRules
{
    public class FirstNameRule : ValidationRule
    {
        public const int MinLength = 2;
        public const int MaxLength = 32;

        public ValidationResult Validate(object value) => Validate(value, Thread.CurrentThread.CurrentCulture);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == null)
                throw new ArgumentException("value argument is not string");

            string firstName = (string)value;

            if (string.IsNullOrEmpty(firstName))
                return new ValidationResult(false, Resource.giveFirstName);

            if (firstName.Length < MinLength)
                return new ValidationResult(false, Resource.firstNameIsTooShort);

            if (firstName.Length > MaxLength)
                return new ValidationResult(false, Resource.firstNameIsTooLong);

            return ValidationResult.ValidResult;
        }
    }
}
