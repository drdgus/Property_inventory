using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;
using Property_inventory.Services.View;

namespace Property_inventory.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        public SettingsWindow()
        {
            InitializeComponent();
            Theme();
            GLOBAL.Event += Theme;
        }

        private void Theme()
        {
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
            if (Properties.Settings.Default.Theme)
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

            Properties.Settings.Default.Theme = true;
            Properties.Settings.Default.Save();
            GLOBAL.UpdateTheme();
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Theme = false;
            Properties.Settings.Default.Save();
            GLOBAL.UpdateTheme();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PasswordBox_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.DataContext != null)
            {
                PasswordBox.Password = ((dynamic)this.DataContext).Password;
            }
        }
    }
}
