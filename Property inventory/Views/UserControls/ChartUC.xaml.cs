using LiveCharts;
using System;
using System.Windows.Controls;

namespace Property_inventory.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ChartUC.xaml
    /// </summary>
    public partial class ChartUC : UserControl
    {
        public ChartUC()
        {
            InitializeComponent();

            YFormatter = value => value.ToString("C");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

    }
}

