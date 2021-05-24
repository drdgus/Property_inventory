using System.Windows;
using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;

namespace Property_inventory.Views.Acts
{
    /// <summary>
    /// Логика взаимодействия для ActOfHandoverToPersonWindow.xaml
    /// </summary>
    public partial class ActOfHandoverToPersonWindow : Window
    {
        public ActOfHandoverToPersonWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }
    }
}
