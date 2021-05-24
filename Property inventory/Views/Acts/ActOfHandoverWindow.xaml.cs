using System.Windows;
using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;

namespace Property_inventory.Views.Acts
{
    /// <summary>
    /// Логика взаимодействия для ActOfHandoverWindow.xaml
    /// </summary>
    public partial class ActOfHandoverWindow : Window
    {
        public ActOfHandoverWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }
    }
}
