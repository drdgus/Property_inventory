using LiveCharts;
using LiveCharts.Wpf;
using Property_inventory.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace Property_inventory.ViewModels
{
    public class ChartVM
    {
        public SeriesCollection CurrentBalanceCollection { get; set; }
        public SeriesCollection ProcurementCollection { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public string[] Labels { get; set; }

        public List<ChartTypes> ChartTypes { get; set; }
        public decimal TotalSum { get; set; }

        public ChartVM()
        {
            YFormatter = value => value.ToString("C");

            TotalSum = 38630200m;
            ChartTypes = new List<ChartTypes>
            {
                new ChartTypes
                {
                    Sum = 2314004,
                    Type = "МФУ",
                    Count = 150
                }
            };

            var vals = new ChartValues<decimal>();
            var rnd = new Random();
            for (int i = 0; i < 11; i++)
            {
                vals.Add(rnd.Next(36000000, 38000000));
            }

            vals.Add(TotalSum);

            CurrentBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "На балансе",
                    Values = vals
                }
            };

            vals = new ChartValues<decimal>();
            vals.Add(0);
            vals.Add(0);
            vals.Add(rnd.Next(50000, 500000));
            vals.Add(0);
            for (int i = 0; i < 7; i++)
            {
                vals.Add(0);
            }

            vals.Add(rnd.Next(50000, 500000));

            ProcurementCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Закупки",
                    Values = vals,
                    Fill = Brushes.LightPink,
                    Stroke = Brushes.IndianRed
                }
            };
            Labels = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToArray();
        }
    }
}
