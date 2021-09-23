using LiveCharts;
using LiveCharts.Wpf;
using Property_inventory.DAL.Repositories;
using Property_inventory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using Property_inventory.DAL.Repositories;
using Property_inventory.Entities;
using Property_inventory.Services;

namespace Property_inventory.ViewModels
{
    public class ChartVM
    {
        public SeriesCollection CurrentBalanceCollection { get; set; }
        public SeriesCollection ProcurementCollection { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string[] Labels { get; set; }

        public List<ChartTypes> ChartTypes { get; set; }
        private List<Equip> EquipList { get; set; }
        public decimal TotalSum { get; set; }

        public ChartVM()
        {
            //YFormatter = value => value.ToString("C");
            //var Equips = new EquipRepository().GetEquip();


            //TotalSum = Equips.Sum(e => e.BasePrice * e.Count);
            //ChartTypes = new List<ChartTypes>();

            //var groups = Equips.GroupBy(e => e.Type.Name).ToList();
            //foreach (var group in groups)
            //{
            //    ChartTypes.Add(new ChartTypes
            //    {
            //        Sum = group.Sum(i => i.BasePrice * i.Count),
            //        Type = new Entities.Type
            //        {
            //            Id = 0,
            //            Category = group.First().Type.Category,
            //            Name = group.First().Type.Name
            //        },
            //        Count = group.Count()
            //    });
            //}

            InitCharts();
        }

            //var currentBalance = new ChartValues<decimal>();
            //var rnd = new Random();
            //for (int i = 0; i < 11; i++)
            //{
            //    currentBalance.Add(rnd.Next(120000, 150000));
            //}

            //currentBalance.Add(TotalSum);

            //CurrentBalanceCollection = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "На балансе",
            //        Values = currentBalance
            //    }
            //};

            //var procurement = new ChartValues<decimal>();
            //procurement.Add(0);
            //procurement.Add(0);
            //procurement.Add(rnd.Next(5000, 12000));
            //procurement.Add(0);
            //for (int i = 0; i < 7; i++)
            //{
            //    procurement.Add(0);
            //}

            //procurement.Add(TotalSum - currentBalance[10]);

            //ProcurementCollection = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Закупки",
            //        Values = procurement,
            //        Fill = Brushes.LightPink,
            //        Stroke = Brushes.IndianRed
            //    }
            //};
            //Labels = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToArray();
        }
    }
}
