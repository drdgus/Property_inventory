using System.Windows;
using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;

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
    }
}
