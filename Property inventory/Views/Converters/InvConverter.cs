using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Property_inventory.Properties;

namespace Property_inventory.Views.Converters
{
    public class InvConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString($"{Settings.Default.InvSymbol}-0000000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}