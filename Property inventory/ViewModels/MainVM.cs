using MaterialDesignThemes.Wpf;
using Property_inventory.DAL;
using Property_inventory.DAL.Repositories;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.Models;
using Property_inventory.Properties;
using Property_inventory.Services;
using Property_inventory.ViewModels.Dialogs;
using Property_inventory.Views;
using Property_inventory.Views.Acts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Property_inventory.DAL.Repositories;
using Property_inventory.Properties;
using Property_inventory.Services.Tools;
using InvType = Property_inventory.Entities.InvType;
using System.Data.Entity.Migrations;
using Property_inventory.Views.Dictionaries;
using System.Windows;

namespace Property_inventory.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        public MainVM()
        {
            new SyncData();
            //View = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(Nodes);
            //View.IsLiveSorting = true;
            CurrentRoomEquip = new ObservableCollection<Equip>();
            EquipTypes = new ObservableCollection<InvType>(new DictionaryRepository().GetTypes());
            //DepreciationGroups = new ObservableCollection<string>(Enum.GetNames(typeof(InvEnums.DepreciationGroups)));
            EquipMOLs = new ObservableCollection<MOL>(new DictionaryRepository().GetMOLs());
            EquipMOLs.Insert(0, new MOL());
            EquipList = new EquipRepository().GetEquip();
            Categories = new ObservableCollection<Category>(new DictionaryRepository().GetCategories());
            AllEquip = new ObservableCollection<EquipInfo>();
            CurrentRoomEquip = new ObservableCollection<Equip>();
            Rooms = new ObservableCollection<Node>();
            AllowCloseOnClickAway = false;
            new SyncData();
        }

        private ObservableCollection<Node> _rooms;
        private Node _selectedNode;
        private string _searchText;
        private Equip _newEquip;
        private bool _infoDialogIsOpen;
        private bool _deleteDialogIsOpen;
        private string _messageDialogContent;
        private string _newName;
        private string _equipDialogOperationContent;
        private int _selectedTypeIndex = 0;
        private int _selectedDeprGroupIndex;
        private string _authMessage;
        private string _password;
        private bool _allowCloseOnClickAway;
        private InvType selectedType;

        public bool InfoDialogIsOpen
        {
            get => _infoDialogIsOpen;
            set
            {
                _infoDialogIsOpen = value;
                OnPropertyChanged();
            }
        }
        public bool DeleteDialogIsOpen
        {
            get => _deleteDialogIsOpen;
            set
            {
                _deleteDialogIsOpen = value;
                OnPropertyChanged();
            }
        }
        public string MessageDialogContent
        {
            get => _messageDialogContent;
            set
            {
                _messageDialogContent = value;
                OnPropertyChanged();
            }
        }
        public Node SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;

                CurrentRoomEquip.Clear();
                var equip = new EquipRepository().GetEquipByRoom(_selectedNode.RoomId, SearchText)
                    .ToList();
                equip.ForEach(i => CurrentRoomEquip.Add(i));
                OnPropertyChanged();
            }
        }
        public Equip SelectedEquip { get; set; }
        public InvType SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                OnPropertyChanged();
            }
        }
        public int SelectedTypeIndex
        {
            get => _selectedTypeIndex;
            set
            {
                _selectedTypeIndex = value;
                OnPropertyChanged();
            }
        }
        public Equip NewEquip
        {
            get
            {
                var allEquips = InvDbContext.GetInstance().Equips.ToList();
                return _newEquip ?? (_newEquip = new Equip
                {
                    RegistrationDate = DateTime.Now,
                    Name = "",
                    InvNum = "",
                    RoomId = 1,
                    InvTypeId = 1,
                    StatusId = 1,
                    AccountabilityId = 2,
                    History = null,
                    Note = "",
                    Count = 1,
                    IsWriteOff = false,
                    MOLId = 1,
                    //ReleaseDate = DateTime.Now,
                    //BasePrice = 0.0m,
                    //DepreciationGroup = InvEnums.DepreciationGroups.I,
                    //BaseInvNum = "",
                    //ManufacturerId = 0,
                });
            }
            private set
            {
                _newEquip = value;
                OnPropertyChanged();
            }
        }
        //public Manufacturer NewManufacturer { get; set; } = new Manufacturer();
        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                OnPropertyChanged();
            }
        }
        public string EquipDialogOperationContent
        {
            get => _equipDialogOperationContent;
            set
            {
                _equipDialogOperationContent = value;
                OnPropertyChanged();
            }
        }
        //public int SelectedTypeIndex
        //{
        //    get => EquipTypes.IndexOf(EquipTypes.Single(t => t.Id == SelectedEquip.Type.Id));
        //    set
        //    {
        //        _selectedTypeIndex = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public int SelectedDeprGroupIndex
        //{
        //    get => (int)SelectedEquip.DepreciationGroup;
        //    set
        //    {
        //        _selectedDeprGroupIndex = value;
        //        OnPropertyChanged();
        //    }
        //}


        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                AuthMessage = "";
            }
        }

        public string AuthMessage
        {
            get => _authMessage;
            set
            {
                _authMessage = value;
                OnPropertyChanged();
            }
        }

        public bool AllowCloseOnClickAway
        {
            get => _allowCloseOnClickAway;
            set
            {
                _allowCloseOnClickAway = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Node> Rooms
        {
            get => _rooms;
            set => _rooms = value;
        }
        public ObservableCollection<Equip> CurrentRoomEquip { get; set; }
        public ObservableCollection<InvType> EquipTypes { get; set; }
        //public ObservableCollection<string> DepreciationGroups { get; set; }
        public ObservableCollection<MOL> EquipMOLs { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        private List<Equip> EquipList { get; set; }
        public ObservableCollection<EquipInfo> AllEquip { get; set; }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;

                var lowerText = _searchText.ToLower();

                var filteredEquip = new List<Equip>();
                if (string.IsNullOrWhiteSpace(lowerText))
                {
                    if (SelectedNode != null)
                    {
                        CurrentRoomEquip.Clear();
                        EquipList.Where(i => i.RoomId == SelectedNode.RoomId).ToList().ForEach(i => CurrentRoomEquip.Add(i));
                    }
                    else
                    {
                        AllEquip.Clear();
                        EquipList.ForEach(i => AllEquip.Add(new EquipInfo
                        {
                            Id = i.Id,
                            RegistrationDate = i.RegistrationDate,
                            Name = i.Name,
                            InvNum = i.InvNum,
                            RoomId = i.RoomId,
                            Room = i.Room,
                            InvType = i.InvType,
                            Status = i.Status,
                            Accountability = i.Accountability,
                            History = i.History,
                            Note = i.Note,
                            Count = i.Count,
                            IsWriteOff = i.IsWriteOff,
                            MOL = i.MOL,
                            //ReleaseDate = i.ReleaseDate,
                            //BasePrice = i.BasePrice,
                            //DepreciationRate = i.DepreciationRate,
                            //DepreciationGroup = i.DepreciationGroup,
                            //BaseInvNum = i.BaseInvNum
                        }));
                    }

                    return;
                }

                filteredEquip = EquipList.Where(i => i.InvNum.ToLower().Contains(lowerText) ||
                                    i.Name.ToLower().Contains(lowerText) ||
                                    //i.MOL.FullName.ToLower().Contains(value) ||
                                    i.Room.Name.ToLower().Contains(lowerText)).ToList();

                AllEquip.Clear();
                filteredEquip.ForEach(i => AllEquip.Add(new EquipInfo
                {
                    Id = i.Id,
                    RegistrationDate = i.RegistrationDate,
                    Name = i.Name,
                    InvNum = i.InvNum,
                    RoomId = i.RoomId,
                    Room = i.Room,
                    InvType = i.InvType,
                    Status = i.Status,
                    Accountability = i.Accountability,
                    History = i.History,
                    Note = i.Note,
                    Count = i.Count,
                    IsWriteOff = i.IsWriteOff,
                    MOL = i.MOL,
                    //ReleaseDate = i.ReleaseDate,
                    //BasePrice = i.BasePrice,
                    //DepreciationRate = i.DepreciationRate,
                    //DepreciationGroup = i.DepreciationGroup,
                    //BaseInvNum = i.BaseInvNum
                }));

                CurrentRoomEquip.Clear();
                if (SelectedNode is null) return;
                filteredEquip.ForEach(i =>
                {
                    if (i.RoomId == SelectedNode.RoomId) CurrentRoomEquip.Add(i);
                });


                //filteredEquip.Where(i => !Nodes[0].Nodes.Contains(new Node
                //{
                //    Name = null,
                //    IsExpanded = false,
                //    SortIndex = 0,
                //    RoomId = 0,
                //    Nodes = null
                //})).ToList().ForEach(i => Nodes[0].Nodes.Add(new Node
                //{
                //    Name = i.Room.Name,
                //    IsExpanded = false,
                //    RoomId = i.Room.Id,
                //    Nodes = new ObservableCollection<Node>()
                //}));
                SelectedEquip = null;
                OnPropertyChanged();
            }
        }

        private async void Authentication()
        {
            //var create = new AuthUC()
            //{
            //    DataContext = this
            //};

            //await DialogHost.Show(create, "RootDialog", ClosingEventHandler);

            AllowCloseOnClickAway = true;
            Init();
        }

        private void Init()
        {
            //View = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(Nodes);
            //View.IsLiveSorting = true;
            EquipTypes = new ObservableCollection<InvType>(new DictionaryRepository().GetTypes());
            //DepreciationGroups = new ObservableCollection<string>(Enum.GetNames(typeof(InvEnums.DepreciationGroups)));
            EquipMOLs = new ObservableCollection<MOL>(new DictionaryRepository().GetMOLs());
            EquipMOLs.Insert(0, new MOL());
            //Rooms = new RoomRepository().Get();
            EquipList = new EquipRepository().GetEquip();
            Categories = new ObservableCollection<Category>(new DictionaryRepository().GetCategories());
            LoadRooms();
        }

        private void LoadRooms()
        {
            var rooms = new ObservableCollection<Node>();
            new RoomRepository().Get().ForEach(i => rooms.Add(new Node
            {
                Name = i.Name,
                RoomId = i.Id,
                IsExpanded = false,
                SortIndex = 0,
            }));

            Rooms.Add(new Node
            {
                Name = InvDbContext.GetInstance().Orgs.Single().Name,
                RoomId = -1,
                SortIndex = 0,
                Nodes = rooms
            });
        }

        #region Commands
        public ICommand CreateRoomCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new CreateRoomUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                    if ((bool)result)
                    {
                        if (string.IsNullOrWhiteSpace(NewName)) return;

                        var newRoom = new Room
                        {
                            Name = NewName,
                            OrgId = Rooms.Single().RoomId,
                            IsDeleted = false
                        };

                        newRoom.Id = new RoomRepository().Add(newRoom);

                        Rooms[0].Nodes.Add(new Node
                        {
                            Name = NewName,
                            IsExpanded = false,
                            SortIndex = 0,
                            RoomId = newRoom.Id,
                            Nodes = new ObservableCollection<Node>()
                        });
                        NewName = string.Empty;
                    }
                });
            }
        }
        public ICommand CreateEquipCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    if (SelectedNode == null || SelectedNode.RoomId <= 0)
                    {
                        var info = new InfoDialog()
                        {
                            DataContext = this
                        };
                        MessageDialogContent = "Выберите помещение.";

                        await DialogHost.Show(info, "RootDialog", ClosingEventHandler);
                        return;
                    }

                    EquipTypes = new ObservableCollection<InvType>(new DictionaryRepository().GetTypes());
                    SelectedType = EquipTypes.First();

                    var create = new CreateEquipDialog()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(create, "RootDialog", ClosingEventHandler);
                    if (result is null || (string)result == "False") return;

                    if (SelectedType.Id == 0) return;
                    if (string.IsNullOrWhiteSpace(NewEquip.Name)) return;
                    if (NewEquip.Count <= 0) return;

                    #region Extended Features

                    //var createManufacturer = new AddManufacturerUC()
                    //{
                    //    DataContext = this
                    //};

                    //var resultManufacturer = await DialogHost.Show(createManufacturer, "RootDialog", ClosingEventHandler);
                    //if (resultManufacturer is null || !(bool)resultManufacturer) return;

                    //if (new ManufacturerRepository().Contains(NewManufacturer))
                    //    NewEquip.ManufacturerId = NewManufacturer.Id;
                    //else
                    //NewEquip.Manufacturer = NewManufacturer;
                    #endregion

                    NewEquip.InvTypeId = SelectedType.Id;
                    NewEquip.RoomId = SelectedNode.RoomId;

                    var i = new EquipRepository().Add(NewEquip);

                    new ActVM(i).PrintSupplyActCommand.Execute(null);

                    CurrentRoomEquip.Add(i);
                    EquipList.Add(i);
                    NewEquip = null;
                    // NewManufacturer = new Manufacturer();
                    ClearCreateEquipCommand.Execute(null);

                    if ((string)result == "Repeat") CreateEquipCommand.Execute(null);
                });
            }
        }

        public ICommand AuthCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if (Password != "Android")
                    {
                        AuthMessage = "Введен неверный пароль";
                        return;
                    }

                    DialogHost.Close("RootDialog");
                });
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    Authentication();
                });
            }
        }

        public ICommand ClearCreateEquipCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    NewEquip = null;
                });
            }
        }

        public ICommand EditRoomCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new RenameUC()
                    {
                        DataContext = this
                    };

                    if (SelectedNode == null) return;

                    var oldName = SelectedNode.Name;
                    NewName = oldName;

                    var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                    if ((bool)result)
                    {
                        if (NewName == oldName) return;
                        if (string.IsNullOrWhiteSpace(NewName)) return;

                        SelectedNode.Name = NewName;
                        if (SelectedNode.RoomId != -1)
                        {
                            new RoomRepository().Update(new Room
                            {
                                Id = _selectedNode.RoomId,
                                Name = _selectedNode.Name,
                                OrgId = 1,
                                IsDeleted = false
                            });
                        }
                        else
                        {
                            InvDbContext.GetInstance().Orgs.AddOrUpdate(new Org { Id = 1, Name = NewName });
                            InvDbContext.GetInstance().SaveChanges();
                        }
                    }

                    NewName = string.Empty;
                });
            }
        }
        public ICommand EditEquipCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {

                    EquipTypes = new ObservableCollection<InvType>(new DictionaryRepository().GetTypes());
                    SelectedType = SelectedEquip.InvType;
                    SelectedTypeIndex = EquipTypes.IndexOf(EquipTypes.Single(i => i.Id == SelectedEquip.InvTypeId));

                    var create = new EquipEditDialog()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(create, "RootDialog", ClosingEventHandler);
                    if (result is null || !(bool)result) return;

                    if (SelectedType.Id == 0) return;
                    if (string.IsNullOrWhiteSpace(SelectedEquip.Name)) return;
                    if (SelectedEquip.Count <= 0) return;

                    SelectedEquip.InvType = null;
                    SelectedEquip.InvTypeId = SelectedType.Id;

                    var a = SelectedEquip;
                    a.InvType = SelectedType;
                    new EquipRepository().Update(SelectedEquip);
                    //a.InvType = InvDbContext.GetInstance().InvTypes.Single(i => i.Id == SelectedEquip.InvTypeId);
                    var index = CurrentRoomEquip.IndexOf(SelectedEquip);
                    CurrentRoomEquip.RemoveAt(index);
                    CurrentRoomEquip.Insert(index, a);
                });
            }
        }

        public ICommand DeleteRoomCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    if (SelectedNode == null || SelectedNode.RoomId == -1) return;

                    var view = new DeleteDialog
                    {
                        DataContext = this
                    };
                    MessageDialogContent = "Удалить выбранное помещение?";

                    var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                    if ((bool)result)
                    {
                        new RoomRepository().Remove(SelectedNode.RoomId);

                        Rooms[0].Nodes.Remove(SelectedNode);
                    }
                });
            }
        }
        //public ICommand DeleteEquipCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(async o =>
        //        {
        //            var view = new DeleteDialog
        //            {
        //                DataContext = this
        //            };
        //            MessageDialogContent = "Удалить выбранное имущество?";

        //            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        //            if ((bool)result)
        //            {
        //                new EquipRepository().Remove(SelectedEquip);
        //                CurrentRoomEquip.Remove(SelectedEquip);
        //            }
        //        });
        //    }
        //}

        public ICommand PrintQRCodesCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Dialog
                    //throw new NotImplementedException();
                });
            }
        }
        public ICommand OpenAllEquipCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    var view = new AllEquipDialog()
                    {
                        DataContext = this
                    };

                    AllEquip.Clear();

                    EquipList.ForEach(i => AllEquip.Add(new EquipInfo
                    {
                        Id = i.Id,
                        RegistrationDate = i.RegistrationDate,
                        Name = i.Name,
                        InvNum = i.InvNum,
                        //RoomId = i.RoomId,
                        Room = i.Room,
                        InvType = i.InvType,
                        Status = i.Status,
                        Accountability = i.Accountability,
                        //History = i.History,
                        Note = i.Note,
                        Count = i.Count,
                        IsWriteOff = i.IsWriteOff,
                        //MOL = i.MOL,
                        //ReleaseDate = i.ReleaseDate,
                        //BasePrice = i.BasePrice,
                        //DepreciationRate = i.DepreciationRate,
                        //DepreciationGroup = i.DepreciationGroup,
                        //BaseInvNum = i.BaseInvNum,
                        //Free = i.RoomId == 0 ? 1 : 0,
                    }));

                    DialogHost.Show(view, "RootDialog");
                });
            }
        }
        public ICommand OpenWriteoffEquipCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    var view = new AllEquipDialog()
                    {
                        DataContext = this
                    };

                    AllEquip.Clear();

                    new EquipRepository().GetDeletedEquip().ForEach(i => AllEquip.Add(new EquipInfo
                    {
                        Id = i.Id,
                        RegistrationDate = i.RegistrationDate,
                        Name = i.Name,
                        InvNum = i.InvNum,
                        //RoomId = i.RoomId,
                        Room = i.Room,
                        InvType = i.InvType,
                        Status = i.Status,
                        Accountability = i.Accountability,
                        //History = i.History,
                        Note = i.Note,
                        Count = i.Count,
                        IsWriteOff = i.IsWriteOff,
                        //MOL = i.MOL,
                        //ReleaseDate = i.ReleaseDate,
                        //BasePrice = i.BasePrice,
                        //DepreciationRate = i.DepreciationRate,
                        //DepreciationGroup = i.DepreciationGroup,
                        //BaseInvNum = i.BaseInvNum,
                        //Free = i.RoomId == 0 ? 1 : 0,
                    }));

                    DialogHost.Show(view, "RootDialog");
                });
            }
        }
        public ICommand OpenEquipHistoryCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new HistoryWindow() { DataContext = new HistoryVM(SelectedEquip.Id) }.ShowDialog();
                });
            }
        }
        public ICommand OpenHistoryCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new HistoryWindow() { DataContext = new HistoryVM() }.ShowDialog();
                });
            }
        }
        public ICommand OpenDicTypesCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Dialog
                    new DictionaryTypesWindow().ShowDialog();
                    EquipTypes.Clear();
                    new DictionaryRepository().GetTypes().ForEach(i => EquipTypes.Add(i));
                });
            }
        }
        public ICommand OpenDicCategoriesCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Dialog
                    new DictionaryCaregoriesWindow().ShowDialog();
                    EquipTypes.Clear();
                    new DictionaryRepository().GetCategories().ForEach(i => Categories.Add(i));
                });
            }
        }
        public ICommand OpenDicMOLsCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Dialog
                    new DictionaryMOLsWindow().ShowDialog();
                    EquipMOLs.Clear();
                    new DictionaryRepository().GetMOLs().ForEach(i => EquipMOLs.Add(i));
                });
            }
        }

        //public ICommand HandoverCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(o =>
        //        {
        //            new ActOfHandoverWindow().ShowDialog();
        //        });
        //    }
        //}
        //public ICommand AllEquipActCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(o =>
        //        {
        //            new AllEquipAct().ShowDialog();
        //        });
        //    }
        //}
        //public ICommand HandoverMOLCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(o =>
        //        {
        //            new ActOfHandoverToPersonWindow().ShowDialog();
        //        });
        //    }
        //}

        //public ICommand OpenInvCardWinCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(o =>
        //        {
        //            new InvCardWindow(){ DataContext = new ActVM(SelectedEquip)}.ShowDialog();
        //        });
        //    }
        //}
        public ICommand OpenRelocateWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    var res = new ActOfRelocateWindow() { DataContext = new ActVM(SelectedEquip) }.ShowDialog();
                    if (res != null && res == true) CurrentRoomEquip.Remove(SelectedEquip);
                });
            }
        }
        public ICommand OpenWriteOffWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    var res = new WriteOffWindow() { DataContext = new ActVM(SelectedEquip) }.ShowDialog();
                    if (res != null && res == true)
                    {
                        var a = SelectedEquip;
                        CurrentRoomEquip.Remove(a);
                        EquipList.Remove(a);

                        var equip = AllEquip.SingleOrDefault(i => i.Id == a.Id);
                        if (equip != null)
                            AllEquip.Remove(equip);
                    }
                });
            }
        }

        #endregion

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
