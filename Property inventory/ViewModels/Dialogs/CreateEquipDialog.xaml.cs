using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Property_inventory.ViewModels.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для EquipDialog.xaml
    /// </summary>
    public partial class CreateEquipDialog : UserControl
    {
        public CreateEquipDialog()
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

        private static readonly Regex _regex = new Regex(@"[^0-9]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void Price_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (((TextBox)sender).Text.Contains(".") && e.Text == ".")
            {
                e.Handled = true;
                return;
            }

            if (e.Text == ".")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = !IsTextAllowed(e.Text);
            }
        }
    }
}
