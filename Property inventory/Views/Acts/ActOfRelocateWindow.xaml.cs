using System.Windows;
using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;

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
    }
}
