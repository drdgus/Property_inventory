using System;
using System.Globalization;
using System.Windows.Data;

namespace Property_inventory.Views.Converters
{
    class CodeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = "";
            switch (System.Convert.ToInt16(value))
            {
                // Добавлен
                case 1:
                    color = "#FFD3FFCF";
                    break;
                // Изменен
                case 2:
                    color = "#FFFDFFD3";
                    break;
                // Удален
                case 3:
                    color = "#FFFFD3CD";
                    break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
