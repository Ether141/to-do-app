using System;
using System.Threading;
using System.Globalization;
using System.Windows.Controls;
using System.Net.Mail;
using ToDoAppClient.Resources.Strings;

namespace ToDoAppClient.Core.ValidationRules
{
    public class EmailRule : ValidationRule
    {
        public ValidationResult Validate(object value) => Validate(value, Thread.CurrentThread.CurrentCulture);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == null)
                throw new ArgumentException("value argument is not string");

            string email = (string)value;
            if (!MailAddress.TryCreate(email, out _))
                return new ValidationResult(false, Resource.wrongEmail);

            return ValidationResult.ValidResult;
        }
    }
}
