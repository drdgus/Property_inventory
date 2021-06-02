using MaterialDesignThemes.Wpf;
using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.Models;
using Property_inventory.Services;
using Property_inventory.ViewModels.Dialogs;
using Property_inventory.Views;
using Property_inventory.Views.Acts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Property_inventory.DAL.Repositories;
using Type = Property_inventory.Entities.Type;

namespace Property_inventory.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        private ObservableCollection<Node> _nodes;
        private Node _selectedNode;
        private string _searchText;
        private Equip _newEquip;
        private bool _infoDialogIsOpen;
        private bool _deleteDialogIsOpen;
        private string _messageDialogContent;
        private string _newName;
        private string _equipDialogOperationContent;
        private int _selectedTypeIndex;
        private int _selectedDeprGroupIndex;

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
                new EquipRepository().GetEquipByRoom(_selectedNode.RoomId).ForEach(i => CurrentRoomEquip.Add(i));
                OnPropertyChanged();
            }
        }

        public Equip SelectedEquip { get; set; }

        public Equip NewEquip
        {
            get =>
                _newEquip ?? (_newEquip = new Equip
                {
                    Id = 0,
                    RegistrationDate = DateTime.Now,
                    Name = "",
                    InvNum = 0,
                    RoomId = 0,
                    TypeId = 0,
                    StatusId = 0,
                    AccountabilityId = 0,
                    History = null,
                    Note = "",
                    Count = 1,
                    IsDeleted = false,
                    MOLId = 0,
                    ReleaseDate = default,
                    BasePrice = 0,
                    DepreciationRate = 0,
                    DepreciationGroup = InvEnums.DepreciationGroups.I,
                    BaseInvNum = "",
                    ManufacturerId = 0,
                });
            private set => _newEquip = value;
        }

        public ObservableCollection<Node> Nodes
        {
            get
            {
                if (_nodes is null)
                {
                    var nodes = new ObservableCollection<Node>();
                    new RoomRepository().Get().ForEach(i => nodes.Add(new Node
                    {
                        Name = i.Name,
                        RoomId = i.Id,
                        IsExpanded = false,
                        SortIndex = 0,
                    }));

                    _nodes = new ObservableCollection<Node>
                    {
                        new Node
                        {
                            Name = InvDbContext.GetInstance().Orgs.Single().Name,
                            RoomId = -1,
                            SortIndex = 0,
                            Nodes = nodes
                        }
                    };
                };
                return _nodes;
            }
            set => _nodes = value;
        }

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
        public int SelectedTypeIndex
        {
            get => EquipTypes.IndexOf(EquipTypes.Single(t => t.Id == SelectedEquip.Type.Id));
            set
            {
                _selectedTypeIndex = value;
                OnPropertyChanged();
            }
        }
        public int SelectedDeprGroupIndex
        {
            get => (int)SelectedEquip.DepreciationGroup;
            set
            {
                _selectedDeprGroupIndex = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Equip> CurrentRoomEquip { get; set; }
        public ObservableCollection<Type> EquipTypes { get; set; }
        public ObservableCollection<string> DepreciationGroups { get; set; }
        public ObservableCollection<MOL> EquipMOLs { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public string InvSymbol { get; set; }

        private List<Room> Rooms { get; set; }
        private List<Equip> EquipList { get; set; }

        public ObservableCollection<EquipInfo> AllEquip { get; set; }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                Nodes[0].Nodes.Clear();
                Rooms.Where(i => i.Name.Contains(value)).ToList().ForEach(i => Nodes[0].Nodes.Add(new Node
                {
                    Name = i.Name,
                    IsExpanded = false,
                    RoomId = i.Id,
                    Nodes = new ObservableCollection<Node>()
                }));
                SelectedEquip = null;
                OnPropertyChanged();
            }
        }

        public MainVM()
        {
            new SyncData();
            //View = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(Nodes);
            //View.IsLiveSorting = true;
            CurrentRoomEquip = new ObservableCollection<Equip>();
            EquipTypes = new ObservableCollection<Type>(new DictionaryRepository().GetTypes());
            DepreciationGroups = new ObservableCollection<string>(Enum.GetNames(typeof(InvEnums.DepreciationGroups)));
            EquipMOLs = new ObservableCollection<MOL>(new DictionaryRepository().GetMOLs());
            Rooms = new RoomRepository().Get();
            EquipList = new EquipRepository().GetEquip();
            Categories = new ObservableCollection<Category>(new DictionaryRepository().GetCategories());
            AllEquip = new ObservableCollection<EquipInfo>();
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

                    new EquipRepository().GetEquip()
                        .ForEach(i => AllEquip.Add(new EquipInfo
                        {
                            Id = i.Id,
                            RegistrationDate = i.RegistrationDate,
                            Name = i.Name,
                            InvNum = i.InvNum,
                            RoomId = i.RoomId,
                            Room = i.Room,
                            Type = i.Type,
                            Status = i.Status,
                            Accountability = i.Accountability,
                            History = i.History,
                            Note = i.Note,
                            Count = i.Count,
                            IsDeleted = i.IsDeleted,
                            MOL = i.MOL,
                            ReleaseDate = i.ReleaseDate,
                            BasePrice = i.BasePrice,
                            DepreciationRate = i.DepreciationRate,
                            DepreciationGroup = i.DepreciationGroup,
                            BaseInvNum = i.BaseInvNum,
                            Free = i.RoomId == 0 ? 1 : 0,
                            Used = i.RoomId != 0 ? 1 : 0
                        }));

                    DialogHost.Show(view, "RootDialog");
                });
            }
        }

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
                        var newRoom = new Room
                        {
                            Name = NewName,
                            OrgId = Nodes.Single().RoomId,
                            IsDeleted = false
                        };
                        
                        newRoom .Id = new RoomRepository().Add(newRoom);

                        Nodes[0].Nodes.Add(new Node
                        {
                            Name = NewName,
                            IsExpanded = false,
                            SortIndex = 0,
                            RoomId = newRoom.Id,
                            Nodes = new ObservableCollection<Node>()
                        });

                    }
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

                    var oldName = SelectedNode.Name;
                    NewName = oldName;

                    var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                    if ((bool)result)
                    {
                        if (NewName == oldName) return;

                        SelectedNode.Name = NewName;
                        new RoomRepository().Update(new Room
                        {
                            Id = _selectedNode.RoomId,
                            Name = _selectedNode.Name,
                            OrgId = 1,
                            IsDeleted = false
                        });
                    }
                });
            }
        }
        public ICommand PrintQRCodesCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Dialog
                    throw new NotImplementedException();
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

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");
        public ICommand DeleteRoomCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new DeleteDialog
                    {
                        DataContext = this
                    };
                    MessageDialogContent = "Удалить выбранное помещение?";

                    var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                    if ((bool)result)
                    {
                        new RoomRepository().Remove(_selectedNode.RoomId);

                        Nodes[0].Nodes.Remove(SelectedNode);
                    }
                });
            }
        }
        public ICommand OpenEquipHistoryCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new HistoryWindow().ShowDialog();
                });
            }
        }
        public ICommand EditEquipCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var create = new EquipEditDialog()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(create, "RootDialog", ClosingEventHandler);
                    if (result is null || !(bool)result) return;

                    new EquipRepository().Update(SelectedEquip);
                });
            }
        }
        public ICommand DeleteEquipCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new DeleteDialog
                    {
                        DataContext = this
                    };
                    MessageDialogContent = "Удалить выбранное имущество?";

                    var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                    if ((bool)result)
                    {
                        new EquipRepository().Remove(SelectedEquip);
                        CurrentRoomEquip.Remove(SelectedEquip);
                    }
                });
            }
        }
        public ICommand HandoverCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new ActOfHandoverWindow().ShowDialog();
                });
            }
        }
        public ICommand AllEquipActCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new AllEquipAct().ShowDialog();
                });
            }
        }
        public ICommand HandoverMOLCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new ActOfHandoverToPersonWindow().ShowDialog();
                });
            }
        }
        public ICommand OpenInvCardWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new InvCardWindow().ShowDialog();
                });
            }
        }
        public ICommand OpenRelocateWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new ActOfRelocateWindow().ShowDialog();
                });
            }
        }
        public ICommand OpenWriteOffWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    new WriteOffWindow().ShowDialog();
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

                    var create = new EquipDialog()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(create, "RootDialog", ClosingEventHandler);
                    if (result is null || !(bool)result) return;

                    NewEquip.RoomId = SelectedNode.RoomId;
                    var molId = NewEquip.MOL.Id;
                    NewEquip.MOLId = molId;
                    
                    var id = new EquipRepository().Add(NewEquip);
                    CurrentRoomEquip.Add(NewEquip);
                    NewEquip = null;
                    ClearCreateEquipCommand.Execute(null);
                    HandoverCommand.Execute(null);
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
