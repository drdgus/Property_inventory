using LiveCharts;
using LiveCharts.Wpf;
using Property_inventory.DAL;
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
            EquipList = new EquipRepository().GetEquip();
            TotalSum = EquipList.Sum(e => e.BasePrice * e.Count);
            ChartTypes = new List<ChartTypes>();

            var groups = EquipList.GroupBy(e => e.Type.Name).ToList();
            foreach (var group in groups)
            {
                ChartTypes.Add(new ChartTypes
                {
                    Sum = group.Sum(i => i.BasePrice * i.Count),
                    Type = new Entities.Type
                    {
                        Id = 0,
                        Category = group.First().Type.Category,
                        Name = group.First().Type.Name
                    },
                    Count = group.Sum(i => i.Count)
                });
            }

            InitCharts();
        }

        private void InitCharts()
        {
            YFormatter = value => value.ToString("C");

            InitProcurementChart();
            InitCurrentBalanceChart();

            Labels = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToArray();
        }

        private void InitCurrentBalanceChart()
        {
            var currentBalance = new ChartValues<decimal>();
            
            var totalPrev = EquipList.Where(e => e.RegistrationDate.Year < DateTime.Now.Year && e.IsWriteOff == false).Sum(e => e.BasePrice + e.Count);
            var equipInCurrentYear = EquipList.Where(e => e.RegistrationDate.Year == DateTime.Now.Year && e.IsWriteOff == false).GroupBy(e => e.RegistrationDate.Month).ToList();

            var currentTotal = totalPrev;
            for (var month = 1; month <= DateTime.Now.Month; month++)
            {
                if(equipInCurrentYear.Any(i => i.Key == month))
                    currentBalance.Add(totalPrev + equipInCurrentYear.Single(i => i.Key == month).Sum(e => e.BasePrice * e.Count));
                else  currentBalance.Add(currentTotal);
            }

            CurrentBalanceCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "На балансе",
                    Values = currentBalance
                }
            };
        }

        private void InitProcurementChart()
        {
            var procurement = new ChartValues<decimal>();

            var equipInCurrentYear = EquipList
                .Where(i => i.RegistrationDate.Year == DateTime.Now.Year)
                .GroupBy(i => i.RegistrationDate.Month)
                .ToList();

            for (var month = 1; month <= DateTime.Now.Month; month++)
            {
                try
                {
                    if (equipInCurrentYear.Any(i => i.Key == month))
                    {
                        var group = equipInCurrentYear.Single(i => i.Key == month);
                        procurement.Add(group.Sum(i => i.BasePrice * i.Count));
                        equipInCurrentYear.Remove(group);
                    }
                    else procurement.Add(0m);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    procurement.Add(0m);
                }
                
            }

            ProcurementCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Закупки   ",
                    Values = procurement,
                    Fill = Brushes.LightPink,
                    Stroke = Brushes.IndianRed
                }
            };
        }
    }
}
