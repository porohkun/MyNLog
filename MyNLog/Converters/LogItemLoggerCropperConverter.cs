using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MyNLog.Converters
{
    public class LogItemLoggerCropperConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return null;
            if (!(values[0] is string logger))
                return null;
            if (!(values[1] is bool fullLength))
                return null;
            if (fullLength)
                return logger;
            else
                return logger.Split('.').Last();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
