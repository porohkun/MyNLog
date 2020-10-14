using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace MyNLog.Converters
{
    [System.Windows.Markup.ContentProperty(nameof(Converters))]
    public class ChainConverter : IValueConverter
    {
        public ObservableCollection<IValueConverter> Converters { get; } = new ObservableCollection<IValueConverter>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = value;

            for (int i = 0; i < Converters.Count; i++)
            {
                IValueConverter converter = Converters[i];
                output = converter.Convert(output, i == (Converters.Count - 1) ? targetType : typeof(object), parameter, culture);

                if (output == Binding.DoNothing)
                    return Binding.DoNothing;
            }

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object output = value;

            for (int i = Converters.Count - 1; i > -1; i--)
            {
                IValueConverter converter = Converters[i];
                output = converter.ConvertBack(output, i == (Converters.Count - 1) ? targetType : typeof(object), parameter, culture);

                if (output == Binding.DoNothing)
                    return Binding.DoNothing;
            }

            return output;
        }
    }
}
