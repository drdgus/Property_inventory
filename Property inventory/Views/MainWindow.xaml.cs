using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;
using Property_inventory.Views.Map;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Property_inventory.Views
{



    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
            GLOBAL.Event += delegate { ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark); };
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            if (e.Text == "." && ((TextBox)sender).Text.Contains("."))
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

        private void EditRoom(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).EditRoomCommand.Execute(null);
        }

        private void PrintQRCodes(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).PrintQRCodesCommand.Execute(null);
        }

        private void DeleteRoom(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).DeleteRoomCommand.Execute(null);
        }

        private void EditEquip(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).EditEquipCommand.Execute(null);
        }

        private void DeleteEquip(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).DeleteEquipCommand.Execute(null);
        }
    }
}
