using System;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;
using Property_inventory.ViewModels;

namespace Property_inventory.Views.Map
{
    /// <summary>
    /// Логика взаимодействия для MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }

        private Point MouseDownLocation;

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            //MouseDownLocation = e.GetPosition(this);
            //Console.WriteLine("Called");
        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            //((MapVM) DataContext).MousePosition = MouseDownLocation;
        }
    }
}
