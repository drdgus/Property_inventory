﻿using System.Windows;
using MaterialDesignThemes.Wpf;
using Property_inventory.Services.View;

namespace Property_inventory.Views.Acts
{
    /// <summary>
    /// Логика взаимодействия для InvCardWindow.xaml
    /// </summary>
    public partial class InvCardWindow : Window
    {
        public InvCardWindow()
        {
            InitializeComponent();
            ThemeAssist.SetTheme(this, Properties.Settings.Default.Theme == false ? BaseTheme.Light : BaseTheme.Dark);
        }
    }
}
