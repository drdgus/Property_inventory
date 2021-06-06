using MaterialDesignThemes.Wpf;
using System.Windows;

namespace Property_inventory.Views
{
    /// <summary>
    /// Логика взаимодействия для DictionariesWindow.xaml
    /// </summary>
    public partial class DictionaryMOLsWindow : Window
    {
        public DictionaryMOLsWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }

        private void DeleteMOL(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).DeleteMOLCommand.Execute(null);
        }

        private void EditMOL(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).EditMOLCommand.Execute(null);
        }
    }
}
