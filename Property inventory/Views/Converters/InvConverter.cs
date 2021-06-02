using Property_inventory.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Property_inventory.Views.Converters
{
    public class InvConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((int)value) == 0) return "-";
            return ((int)value).ToString($"{Settings.Default.InvSymbol}-0000000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return System.Convert.ToInt32(((string)value).Replace($"{Settings.Default.InvSymbol}-", ""));
            return null;
        }
    }
}