using MaterialDesignThemes.Wpf;
using System.Windows;

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
