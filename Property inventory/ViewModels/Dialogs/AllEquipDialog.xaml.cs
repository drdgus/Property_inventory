using System.Windows;
using System.Windows.Controls;

namespace Property_inventory.ViewModels.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AllEquipDialog.xaml
    /// </summary>
    public partial class AllEquipDialog : UserControl
    {
        public AllEquipDialog()
        {
            InitializeComponent();
        }

        private void EditEquip(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).EditEquipCommand.Execute(null);
        }
    }
}
