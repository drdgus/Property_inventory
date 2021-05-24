using Property_inventory.Views.Map;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Property_inventory.ViewModels;
using Property_inventory.Views.Acts;

namespace Property_inventory.Views
{

    public delegate void delegateTheme();

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void CmAddEquip_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmRenameRoom_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmDelRoom_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmQRcodes_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmAddRoom_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmDelOrg_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmRenameOrg_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CmAddOrg_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MiTypes_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiStatuses_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiAccountability_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiHisttory_Click(object sender, RoutedEventArgs e)
        {
            new HistoryWindow().ShowDialog();
        }

        private void MiByOrg_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiByRoom_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MiSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }

        private void MiDeveloper_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            new MapWindow().ShowDialog();
        }

        private void BtnStat_OnClick(object sender, RoutedEventArgs e)
        {
            new ChartWindow().ShowDialog();
        }

        private void mi_Handover(object sender, RoutedEventArgs e)
        {
            new ActOfHandoverWindow().ShowDialog();
        }

        private void mi_HandoverMOL(object sender, RoutedEventArgs e)
        {
            new ActOfHandoverToPersonWindow().ShowDialog();
        }

        private void mi_InvCard(object sender, RoutedEventArgs e)
        {
            new InvCardWindow().ShowDialog();
        }

        private void mi_Relocate(object sender, RoutedEventArgs e)
        {
            new ActOfRelocateWindow().ShowDialog();
        }

        private void mi_WriteOff(object sender, RoutedEventArgs e)
        {
            new WriteOffWindow().ShowDialog();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DailogContent.MaxHeight = this.ActualHeight - 100;
        }

        private void CreateEquipBtn(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).CreateEquipCommand.Execute(null);
        }

        private void ClearCreateEquipBtn(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).ClearCreateEquipCommand.Execute(null);
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

        private void DeleteRoomBtn(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).DeleteRoomCommand.Execute(null);
        }

        private void OpenDeleteRoomDialog(object sender, RoutedEventArgs e)
        {
            ((dynamic) DataContext).DeleteRoomCommand.Execute(null);
        }
    }
}
