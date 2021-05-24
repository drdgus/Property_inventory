using System.Windows;
using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;

namespace Property_inventory.Views.Map
{
    /// <summary>
    /// Логика взаимодействия для MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }
    }
}
