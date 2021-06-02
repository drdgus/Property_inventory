using MaterialDesignThemes.Wpf;
using System.Windows;

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
