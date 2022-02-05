using System;
using System.Threading;
using System.Globalization;
using System.Windows.Controls;
using ToDoAppClient.Resources.Strings;
using System.Text.RegularExpressions;

namespace ToDoAppClient.Core.ValidationRules
{
    public class PasswordRule : ValidationRule
    {
        public const int MinLength = 8;
        public const int MaxLength = 32;
        public readonly Regex HasNumber = new Regex(@"[0-9]+");
        public readonly Regex HasUpperChar = new Regex(@"[A-Z]+");
        public readonly Regex HasLowerChar = new Regex(@"[a-z]+");
        public readonly Regex HasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        public ValidationResult Validate(object value) => Validate(value, Thread.CurrentThread.CurrentCulture);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == null)
                throw new ArgumentException("value argument is not string");

            string password = (string)value;

            if (password.Length < MinLength)
                return new ValidationResult(false, Resource.passwordIsTooShort);

            if (password.Length > MaxLength)
                return new ValidationResult(false, Resource.passwordIsTooLong);

            if (!HasNumber.IsMatch(password))
                return new ValidationResult(false, Resource.passwordMustHasNumber);

            if (!HasUpperChar.IsMatch(password))
                return new ValidationResult(false, Resource.passwordMustHasUpperChar);

            if (!HasLowerChar.IsMatch(password))
                return new ValidationResult(false, Resource.passwordMustHasLowerChar);

            if (!HasSymbols.IsMatch(password))
                return new ValidationResult(false, Resource.passwordMustHasSymbol);

            return ValidationResult.ValidResult;
        }
    }
}
