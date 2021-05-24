using MaterialDesignThemes.Wpf;
using System.Windows;

namespace Property_inventory.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public event delegateTheme Event;

        public SettingsWindow()
        {
            InitializeComponent();
            Theme();
            Event += Theme;
        }

        private void Theme()
        {
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme);
            if (Properties.Settings.Default.Theme == BaseTheme.Dark)
            {
                ToggleButton.IsChecked = true;
                //ToggleButton.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                ToggleButton.IsChecked = false;
                //ToggleButton.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {

            Properties.Settings.Default.Theme = BaseTheme.Dark;
            Properties.Settings.Default.Save();
            Event?.Invoke();
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Theme = BaseTheme.Light;
            Properties.Settings.Default.Save();
            Event?.Invoke();
        }
    }
}
