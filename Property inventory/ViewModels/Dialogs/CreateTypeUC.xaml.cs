using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace Property_inventory.ViewModels.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreateTypeUC.xaml
    /// </summary>
    public partial class CreateTypeUC : UserControl
    {
        public CreateTypeUC()
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
