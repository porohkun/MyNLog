using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MyNLog.Converters
{
    public class LogItemLoggerCropperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string logger)
                return logger.Split('.').Last();
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
