using Property_inventory.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Property_inventory.Views.Converters
{
    internal class NullToIsEnableConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value != null)
            //{
            //    var type = value.GetType();
            //    if (type == typeof(Category))
            //    {
            //        var val = value as Category;
            //        if (string.IsNullOrWhiteSpace(val.Name)) return false;
            //        if (string.IsNullOrWhiteSpace(val.Class)) return false;
            //        return true;
            //    }
            //    else if (type == typeof(InvType))
            //    {
            //        var val = value as InvType;
            //        if (string.IsNullOrWhiteSpace(val.Name)) return false;
            //        if (val.Category == null) return false;
            //        return true;
            //    }
            //    else if (type == typeof(Equip))
            //    {
            //        var val = value as Equip;
            //        if (string.IsNullOrWhiteSpace(val.Name)) return false;
            //        if (val.InvType == null) return false;
            //        if (val.Count <= 0) return false;
            //        return true;
            //    }
            //    else if(type == typeof(string))
            //    {
            //        var val = value as string;
            //        if(string.IsNullOrWhiteSpace(val)) return false;
            //        return true;
            //    }
            //    else
            //        return true;
            //}
            //return false;
            return true;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
