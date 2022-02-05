using System;
using System.Threading;
using System.Globalization;
using System.Windows.Controls;
using ToDoAppClient.Resources.Strings;

namespace ToDoAppClient.Core.ValidationRules
{
    public class SignInPasswordRule : ValidationRule
    {
        public ValidationResult Validate(object value) => Validate(value, Thread.CurrentThread.CurrentCulture);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == null)
                throw new ArgumentException("value argument is not string");

            string login = (string)value;

            if (string.IsNullOrEmpty(login))
                return new ValidationResult(false, Resource.givePassword);

            return ValidationResult.ValidResult;
        }
    }
}
