using System.Windows;
using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;

namespace Property_inventory.Views
{
    /// <summary>
    /// Логика взаимодействия для DictionariesWindow.xaml
    /// </summary>
    public partial class DictionaryTypesWindow : Window
    {
        public DictionaryTypesWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }

        private void DeleteType(object sender, RoutedEventArgs e)
        {
            ((dynamic) DataContext).DeleteTypeCommand.Execute(null);
        }

        private void EditType(object sender, RoutedEventArgs e)
        {
            ((dynamic) DataContext).EditTypeCommand.Execute(null);
        }
    }
}
