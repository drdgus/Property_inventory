﻿using Property_inventory.DAL.Repositories;
using Property_inventory.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Property_inventory.DAL.Repositories;
using Property_inventory.Properties;
using Property_inventory.Services;
using Property_inventory.Services.Tools;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Property_inventory.Infrastructure;
using System.Windows.Input;
using Property_inventory.Views.Acts;
using Property_inventory.Views;
using Property_inventory.Entities;

namespace Property_inventory.ViewModels
{
    public class HistoryVM : INotifyPropertyChanged
    {
        private string _search;
        private string _selectedOperation;
        private string splitter = "->";

        public ObservableCollection<EquipHistory> HistoryListForView { get; set; }
        public List<EquipHistory> AllHistory { get; set; }
        public Dictionary<string, string> Operations { get; set; }

        public Equip SelectedEquip { get; set; }

        public string SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                _selectedOperation = value;
                //AllHistory.Clear();
                //AllHistory.Where(i => i.Code.GetStringValue() == Operations.Single(pair => pair.Value == value).Key).ToList().ForEach(i => HistoryList.Add(i));
                HistoryListForView.Clear();
                Search = "";
                AllHistory.Where(i => _selectedOperation.Contains(i.Code.ToString()))
                    .ToList()
                    .ForEach(i => HistoryListForView.Add(i));
            }
        }

        public string Search
        {
            get => _search;
            set
            {
                _search = value.ToLower();

                HistoryListForView.Clear();
                AllHistory.Where(i => i.InvNum.ToLower().Contains(value) ||
                                      i.Name.ToLower().Contains(value) ||
                                      i.InvNum.ToLower().Contains(value) ||
                                      i.ChangedPropertyStr.ToLower().Contains(value) ||
                                      i.OldValue.ToLower().Contains(value) ||
                                      i.NewValue.ToLower().Contains(value) ||
                                      i.Date.ToString().Contains(value)
                                                           ).ToList().ForEach(i => HistoryListForView.Add(i));
            }
        }

        public HistoryVM(int id = 0)
        {
            HistoryListForView = new ObservableCollection<EquipHistory>();
            AllHistory = new List<EquipHistory>();

            new HistoryRepository().Get(id)
                .Select(i => new EquipHistory
                {
                    ObjectId = i.ObjectId,
                    Date = i.Date,
                    Code = i.Code,
                    ChangedProperty = i.ChangedProperty,
                    OldValue = i.OldValue,
                    NewValue = i.NewValue
                })
                .ToList()
                .ForEach(i =>
                {
                    HistoryListForView.Add(i);
                    AllHistory.Add(i);
                });

            Operations = new Dictionary<string, string>()
            {
                { "", "Все" },
                { "Relocate", "Перемещено" },
                { "OnBalance", "На балансе" },
                { "Deleted", "Списано" }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
