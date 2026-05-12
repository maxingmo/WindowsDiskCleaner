using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WindowsDiskCleaner.App
{
    public sealed class HexBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                return Brushes.Transparent;
            }

            try
            {
                return (Brush)new BrushConverter().ConvertFromString(text);
            }
            catch (FormatException)
            {
                return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
