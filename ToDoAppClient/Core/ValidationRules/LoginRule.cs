using System;
using System.Threading;
using System.Globalization;
using System.Windows.Controls;
using ToDoAppClient.Resources.Strings;

namespace ToDoAppClient.Core.ValidationRules
{
    public class LoginRule : ValidationRule
    {
        public const int MinLength = 4;
        public const int MaxLength = 32;

        public ValidationResult Validate(object value) => Validate(value, Thread.CurrentThread.CurrentCulture);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == null)
                throw new ArgumentException("value argument is not string");

            string login = (string)value;

            if (string.IsNullOrEmpty(login))
                return new ValidationResult(false, Resource.giveLogin);

            if (login.Length < MinLength)
                return new ValidationResult(false, Resource.loginIsTooShort);

            if (login.Length > MaxLength)
                return new ValidationResult(false, Resource.loginIsTooLong);

            return ValidationResult.ValidResult;
        }
    }
}
