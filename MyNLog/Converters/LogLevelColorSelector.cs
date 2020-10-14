using MyNLog.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MyNLog.Converters
{
    public class LogLevelColorSelector : IValueConverter
    {
        public Brush TraceBrush { get; set; }
        public Brush DebugBrush { get; set; }
        public Brush InfoBrush { get; set; }
        public Brush WarnBrush { get; set; }
        public Brush ErrorBrush { get; set; }
        public Brush FatalBrush { get; set; }
        public Brush OffBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is LogLevel level))
                return InfoBrush;

            switch (level)
            {
                case LogLevel.Trace: return TraceBrush;
                case LogLevel.Debug: return DebugBrush;
                case LogLevel.Info: return InfoBrush;
                case LogLevel.Warn: return WarnBrush;
                case LogLevel.Error: return ErrorBrush;
                case LogLevel.Fatal: return FatalBrush;
                case LogLevel.Off: return OffBrush;
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
