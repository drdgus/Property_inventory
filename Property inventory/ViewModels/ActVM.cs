using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Property_inventory.ViewModels
{
    public class ActVM : INotifyPropertyChanged
    {
        private int _selectedEquipIndex;
        public Equip SelectedEquip { get; set; }
        public List<Room> Rooms { get; set; }

        public int SelectedEquipIndex
        {
            get => SelectedEquip != null ? EquipList.IndexOf(EquipList.Single(i => i.Id == SelectedEquip.Id)) : 0;
            set
            {
                _selectedEquipIndex = value;
                OnPropertyChanged();
            }
        }

        public List<Equip> EquipList { get; set; }



        public ICommand PrintInvCardCommand
        {
            get
            {
                return new RelayCommand(o =>
                {

                });
            }
        }

        public ICommand PrintAllEquipActCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new ExcelEditor();
                });
            }
        }

        public ICommand PrintHandoverActCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new ExcelEditor();
                });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
