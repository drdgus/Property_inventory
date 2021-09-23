using Property_inventory.DAL.Repositories;
using Property_inventory.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
            }
        }

        public string Splitter { get; set; }

        public string Search
        {
            get => _search;
            set
            {
                //_search = value;
                //HistoryList.Clear();

                //if (SelectedOperation == Operations[""])
                //{
                //    AllHistory.Where(i => i.InvNum.Contains(value) ||
                //                          i.Name.Contains(value)).ToList().ForEach(i => HistoryList.Add(i));
                //}
                //else if (SelectedOperation == Operations["Created"])
                //{
                //    AllHistory.Where(i => i.InvNum.Contains(value) ||
                //                          i.Name.Contains(value) || i.Code == History.OperationCode.Created).ToList().ForEach(i => HistoryList.Add(i));
                //}
                //else if (SelectedOperation == Operations["Edited"])
                //{
                //    AllHistory.Where(i => i.InvNum.Contains(value) ||
                //                          i.Name.Contains(value) || i.Code == History.OperationCode.Edited).ToList().ForEach(i => HistoryList.Add(i));
                //}
                //else if (SelectedOperation == Operations["Deleted"])
                //{
                //    AllHistory.Where(i => i.InvNum.Contains(value) ||
                //                          i.Name.Contains(value) || i.Code == History.OperationCode.Deleted).ToList().ForEach(i => HistoryList.Add(i));
                //}
            }
        }

        public HistoryVM()
        {
            HistoryList = new ObservableCollection<EquipHistory>();
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



            Operations = new Dictionary<string, string>();
            Operations.Add("", "Все");
            Operations.Add("Created", "Добавлено");
            Operations.Add("Edited", "Изменено");
            Operations.Add("Deleted", "Удалено");
        }
    }
}
