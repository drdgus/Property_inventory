using Property_inventory.DAL.Repositories;
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

        public ObservableCollection<EquipHistory> HistoryListForView { get; set; }
        public List<EquipHistory> AllHistory { get; }
        public Dictionary<string, string> Operations { get; }

        public Equip SelectedEquip { get; set; }

        public string SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                _selectedOperation = value;
                //AllHistory.Clear();
                //AllHistory.Where(i => i.Code.GetStringValue() == Operations.Single(pair => pair.Value == value).Key).ToList().ForEach(i => HistoryList.Add(i));
                Search = "";
                HistoryListForView.Clear();
                if (_selectedOperation.Contains("Все"))
                {
                    AllHistory.ForEach(i => HistoryListForView.Add(i));
                    return;
                }

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
                _search = value;

                var lowerText = _search.ToLower();

                HistoryListForView.Clear();
                AllHistory.Where(i => _selectedOperation.Contains(i.Code.ToString()) && (
                                      i.InvNum.ToLower().Contains(lowerText) ||
                                      i.Name.ToLower().Contains(lowerText) ||
                                      i.InvNum.ToLower().Contains(lowerText) ||
                                      i.ChangedPropertyStr.ToLower().Contains(lowerText) ||
                                      i.OldValue.ToLower().Contains(lowerText) ||
                                      i.NewValue.ToLower().Contains(lowerText) ||
                                      i.Date.ToString().Contains(lowerText)))
                    .ToList()
                    .ForEach(i => HistoryListForView.Add(i));
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
                  .OrderByDescending(i => i.Date)
                  .ToList()
                  .ForEach(i =>
                    {
                        HistoryListForView.Add(i);
                        AllHistory.Add(i);
                    });

            Operations = new Dictionary<string, string>()
            {
                { "", "Все" },
                { "OnBalance", "На балансе" },
                { "Relocate", "Перемещено" },
                { "Edited", "Изменено" },
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
