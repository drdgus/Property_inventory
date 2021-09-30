using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Property_inventory.Views.Dictionaries
{
    /// <summary>
    /// Interaction logic for DictionaryCaregoriesWindow.xaml
    /// </summary>
    public partial class DictionaryCaregoriesWindow : Window
    {
        public DictionaryCaregoriesWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }

        private void DeleteCategory(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).DeleteCategoryCommand.Execute(null);
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).EditCategoryCommand.Execute(null);
        }
    }
}
