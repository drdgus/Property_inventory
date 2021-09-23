using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace Property_inventory.ViewModels.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AuthUC.xaml
    /// </summary>
    public partial class AuthUC : UserControl
    {
        public AuthUC()
        {
            InitializeComponent();
        }

        private void AuthBtn(object sender, RoutedEventArgs e)
        {
            ((dynamic) DataContext).AuthCommand.Execute(null);
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic) DataContext).Password = PasswordBox.Password;
        }
    }
}
