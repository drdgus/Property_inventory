using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Property_inventory.ViewModels.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для EquipDialog.xaml
    /// </summary>
    public partial class EquipEditDialog : UserControl
    {
        public EquipEditDialog()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                e.Handled = !true;
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regexItem = new Regex("^[0-9]*$");

            e.Handled = !regexItem.IsMatch(e.Text);
        }

        private void Price_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Decimal)
                e.Handled = !true;
        }

        private void Price_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "." && ((TextBox) sender).Text.Contains("."))
            {
                e.Handled = true;
                return;
            }
            var regexItem = new Regex("^[0-9.]*$");

            e.Handled = !regexItem.IsMatch(e.Text);
        }
    }
}
