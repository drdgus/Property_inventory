using Property_inventory.DAL.Repositories;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.Models;
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

        public ActVM()
        {
            SelectedEquip = null;

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
        public string SelectedCause { get; set; }
        public Room SelectedNewRoom { get; set; }
        public Supply Supply { get; set; } = new Supply();
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

        public List<Room> Rooms { get; set; } = new RoomRepository().Get();
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
                    //new ExcelEditor().InvCard(SelectedEquip);
                });
            }
        }

        public ICommand PrintRelocateCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //new ExcelEditor().RelocateAct(SelectedEquip, SelectedNewMOL, Reason, SelectedNewRoom);
                    new EquipRepository().Relocate(SelectedEquip, SelectedNewRoom, SelectedNewMOL);
                });
            }
        }

        public ICommand PrintWriteOffCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //new ExcelEditor().WriteOffAct(SelectedEquip, Reason, SelectedCause);
                    new EquipRepository().Decomission(SelectedEquip);
                });
            }
        }

        public ICommand PrintAllEquipActCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //new ExcelEditor().AllEquipAct();
                });
            }
        }

        //public ICommand PrintHandoverActCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(o =>
        //        {
        //            new ExcelEditor().HandoverAct();
        //        });
        //    }
        //}

        public ICommand PrintSupplyActCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Supply = new Supply
                    //{
                    //    SupplierName = "ООО «Техноград»",
                    //    SupplierAddressPhone = "г. Москва, ул. Северная, д. 23, тел. +7(495)234-45-67",
                    //    SupplierRequisites = "ПАО «Бета-банк», р/с  406029876100000000934, к/с 98761111300000000555,  БИК  067111123, ИНН 5679876543, КПП 123456765",
                    //    ManufacturerName = "Hewlett Packard, Вьетнам",
                    //    TransportName = "ООО «Техноград» (поставщик)",
                    //    TransportRequisites = "+7(495)234-45-67, ПАО «Бета-банк», р/с  406029876100000000934, к/с 98761111300000000555,  БИК  067111123, ИНН 5679876543, КПП 123456765",
                    //    FromAddress = "Москва, ул. Северная, 23",
                    //    ToAddress = "Москва, ул. Старостроительная, 67, 09:15",
                    //    CheckStart = DateTime.Now.AddDays(-1),
                    //    CheckEnd = DateTime.Now,
                    //    EquipName = SelectedEquip.Name,
                    //    EquipBaseInvNum = SelectedEquip.BaseInvNum,
                    //    EquipBasePrice = SelectedEquip.BasePrice,
                    //    EquipTotalPrice = SelectedEquip.BasePrice + SelectedEquip.Count
                    //};
                    

                    //new ExcelEditor().SupplyAct(Supply);
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
