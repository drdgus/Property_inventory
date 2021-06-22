using Property_inventory.DAL;
using Property_inventory.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Property_inventory.DAL.Repositories;
using Property_inventory.Properties;
using Property_inventory.Services;
using Property_inventory.Services.Tools;

namespace Property_inventory.ViewModels
{
    public class HistoryVM
    {
        private string _search;
        private string _selectedOperation;
        public ObservableCollection<EquipHistory> HistoryList { get; set; }
        public List<EquipHistory> AllHistory { get; set; }
        public Dictionary<string, string> Operations { get; set; }

        public string SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                _selectedOperation = value;
                //AllHistory.Clear();
                //AllHistory.Where(i => i.Code.GetStringValue() == Operations.Single(pair => pair.Value == value).Key).ToList().ForEach(i => HistoryList.Add(i));
                HistoryList.Clear();
                Search = "";
                AllHistory.Where(i => _selectedOperation.Contains(i.Code.ToString())
                ).ToList().ForEach(i => HistoryList.Add(i));
            }
        }

        public string Splitter { get; set; }

        public string Search
        {
            get => _search;
            set
            {
                _search = value.ToLower();
                
                HistoryList.Clear();
                AllHistory.Where(i => i.InvNum.ToLower().Contains(value) ||
                                      i.Name.ToLower().Contains(value) ||
                                      i.InvNum.ToLower().Contains(value) ||
                                      i.ChangedPropertyStr.ToLower().Contains(value) ||
                                      i.OldValue.ToLower().Contains(value)  ||
                                      i.NewValue.ToLower().Contains(value)
                                                           ).ToList().ForEach(i => HistoryList.Add(i));
            }
        }

        public HistoryVM()
        {
            HistoryList = new ObservableCollection<EquipHistory>();
            AllHistory = new List<EquipHistory>();
            Splitter = "->";
            new HistoryRepository().Get().Select(i => new EquipHistory
            {
                ObjectId = i.ObjectId,
                Date = i.Date,
                Code = i.Code,
                ChangedProperty = i.ChangedProperty,
                OldValue = i.OldValue,
                NewValue = i.NewValue
            }).ToList().ForEach(i => HistoryList.Add(i));

            new HistoryRepository().Get().Select(i => new EquipHistory
            {
                ObjectId = i.ObjectId,
                Date = i.Date,
                Code = i.Code,
                ChangedProperty = i.ChangedProperty,
                OldValue = i.OldValue,
                NewValue = i.NewValue
            }).ToList().ForEach(i => AllHistory.Add(i));



            Operations = new Dictionary<string, string>();
            Operations.Add("", "Все");
            Operations.Add("Relocate", "Перемещено");
            Operations.Add("OnBalance", "На балансе");
            Operations.Add("Deleted", "Списано");
        }
    }
}
