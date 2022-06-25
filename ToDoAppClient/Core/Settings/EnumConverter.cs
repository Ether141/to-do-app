using System;
using System.Globalization;
using System.Windows.Data;

namespace ToDoAppClient.Core.Settings
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            return value.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
}
