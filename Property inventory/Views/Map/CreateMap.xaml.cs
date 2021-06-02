using MaterialDesignThemes.Wpf;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Property_inventory.Views.Map
{
    /// <summary>
    /// Логика взаимодействия для CreateMap.xaml
    /// </summary>
    public partial class CreateMap : Window
    {
        public CreateMap()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }

        private void TbMapName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Space)
                e.Handled = !true;
        }

        private void TbMapName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regexItem = new Regex("^[a-zA-Zа-яА-Я0-9 ]*$");

            e.Handled = !regexItem.IsMatch(e.Text);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Closing -= CreateMapWindow_Closing;
            try
            {
                Close();
            }
            catch { }
        }

        private void CreateMapWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = !add;
        }
    }
}
