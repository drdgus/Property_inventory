using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Property_inventory.Models;
using Type = System.Type;

namespace Property_inventory.Views.Converters
{
    public class SortingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            System.Collections.IList collection = value as System.Collections.IList;
            ListCollectionView view = new ListCollectionView(collection);
            view.IsLiveSorting = true;
            view.IsLiveFiltering = true;
            //view.Filter = o => ((Node)o).Name.Contains(parameter.ToString());
            SortDescription sort = new SortDescription("SortIndex", ListSortDirection.Descending);
            view.SortDescriptions.Add(sort);

            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
