using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace MyNLog.Converters
{
    public class StackTraceToLinesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string input))
                return null;

            var r = new Regex(" => ");
            int tabs = 0;
            return r.Replace(input, (m) => '\n' + Indent(++tabs));
        }

        private string Indent(int tabs)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tabs; i++)
                sb.Append("  ");
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
