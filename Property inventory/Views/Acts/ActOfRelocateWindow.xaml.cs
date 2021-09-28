using MaterialDesignThemes.Wpf;
using System.Windows;

namespace Property_inventory.Views.Acts
{
    /// <summary>
    /// Логика взаимодействия для ActOfRelocateWindow.xaml
    /// </summary>
    public partial class ActOfRelocateWindow : Window
    {
        public ActOfRelocateWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
