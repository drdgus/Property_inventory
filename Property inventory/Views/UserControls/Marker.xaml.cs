using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Property_inventory.Entities;

namespace Property_inventory.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Marker.xaml
    /// </summary>
    public partial class Marker : UserControl
    {
        public Models.Marker MarkerContent { get; set; }

        public Marker()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            DataGrid.Visibility = Visibility.Visible;
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            DataGrid.Visibility = Visibility.Collapsed;
        }
    }
}
