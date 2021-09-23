using Property_inventory.DAL.Repositories;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Property_inventory.ViewModels
{
    public class ActVM : INotifyPropertyChanged
    {
        public ActVM(Equip selectedEquip)
        {
            SelectedEquip = selectedEquip;

            Causes = new List<string>()
            {
                "Моральный и/или физический износ имущественного фонда",
                "Непоправимая порча (умышленная либо случайная)",
                "Хищение основного средства",
                "Утрата объекта, выявленная при инвентаризации",
                "Уничтожение в результате чрезвычайной ситуации (аварии, стихийного бедствия, катастрофы и т.п.)"
            };

        }

        private int _selectedEquipIndex;
        private int _selectedOldMOLIndex;

        public Equip SelectedEquip { get; set; }
        public MOL SelectedNewMOL { get; set; }
        public string SelecteCause { get; set; }
        public string Reason { get; set; }
        public int SelectedEquipIndex
        {
            get => SelectedEquip != null ? EquipList.IndexOf(EquipList.Single(i => i.Id == SelectedEquip.Id)) : 0;
            set
            {
                _selectedEquipIndex = value;
                OnPropertyChanged();
            }
        }
        public int SelectedOldMOLIndex
        {
            get => _selectedOldMOLIndex != null ? EquipList.IndexOf(EquipList.Single(i => i.Id == SelectedEquip.Id)) : 0;
            set
            {
                _selectedOldMOLIndex = value;
                OnPropertyChanged();
            }
        }
        public List<Room> Rooms { get; set; }
        public List<string> Causes { get; set; }
        public List<Equip> EquipList { get; set; } = new EquipRepository().GetEquip();
        public List<MOL> MOLList { get; set; } = new DictionaryRepository().GetMOLs();

        public ICommand PrintInvCardCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //SelectedEquip
                });
            }
        }

        public ICommand PrintRelocateCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //SelectedEquip
                    //SelectedEquip.MOL.ShortFullName
                    //SelectedNewMOL
                    //Reason
                });
            }
        }

        public ICommand PrintWriteOffCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //SelectedEquip
                    //Reason
                    //SelectedCause
                });
            }
        }

        public ICommand PrintAllEquipActCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //new ExcelEditor();
                });
            }
        }

        public ICommand PrintHandoverActCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //new ExcelEditor();
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
