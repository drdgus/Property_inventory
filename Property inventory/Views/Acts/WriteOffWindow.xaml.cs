using MaterialDesignThemes.Wpf;
using System.Windows;

namespace Property_inventory.Views.Acts
{
    /// <summary>
    /// Логика взаимодействия для WriteOffWindow.xaml
    /// </summary>
    public partial class WriteOffWindow : Window
    {
        public WriteOffWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
