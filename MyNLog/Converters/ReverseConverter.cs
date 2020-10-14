using System;
using System.Globalization;
using System.Windows.Data;

namespace MyNLog.Converters
{
    public class ReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool input))
                return true;

            return !input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool input))
                return false;

            return !input;
        }
    }
}
