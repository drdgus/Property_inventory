using System;
using System.Collections.Generic;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Data.Entity;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Property_inventory.DAL;
using Property_inventory.Properties;
using Property_inventory.Services;
using Property_inventory.ViewModels.Dialogs;
using Property_inventory.Views;
using Property_inventory.Views.Acts;
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

        public InvDbContext DbContext { get; set; }
        public Node SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;

                CurrentRoomEquip.Clear();
                DbContext.Equips
                    .AsNoTracking()
                    .Include(i => i.Type)
                    .Include(i => i.Status)
                    .Include(e => e.MOL)
                    .Include(e => e.Type.Category)
                    .Where(i => i.RoomId == _selectedNode.RoomId && i.IsDeleted == false)
                    .ToList()
                    .ForEach(i => CurrentRoomEquip.Add(i));
                OnPropertyChanged();
            }
        }

        public Equip SelectedEquip { get; set; }

        public Equip NewEquip
        {
            get =>
                _newEquip ?? (_newEquip = new Equip
                {
                    RegistrationDate = DateTime.Now,
                    Name = "",
                    InvNum = 0,
                    OrgId = DbContext.Orgs.Single().Id,
                    RoomId = 0,
                    StatusId = DbContext.Statuses.First().Id,
                    Type = new Type(),
                    AccountabilityId = DbContext.Accountabilities.First().Id,
                    History = new List<History>(),
                    Note = "",
                    Count = 1,
                    IsDeleted = false,
                    ReleaseDate = DateTime.Now,
                    BasePrice = 0,
                    DepreciationRate = 0,
                    BaseInvNum = ""
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
                    DbContext.Rooms.AsNoTracking().Where(r => r.IsDeleted == false).ToList().ForEach(i => nodes.Add(new Node
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
                            Name = DbContext.Orgs.Single().Name,
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
            DbContext = InvDbContext.GetInstance();
            //View = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(Nodes);
            //View.IsLiveSorting = true;
            CurrentRoomEquip = new ObservableCollection<Equip>();
            EquipTypes = new ObservableCollection<Type>(DbContext.Types.ToList());
            DepreciationGroups = new ObservableCollection<string>(Enum.GetNames(typeof(Equip.DepreciationGroups)));
            EquipMOLs = new ObservableCollection<MOL>(DbContext.MOLs.AsNoTracking().ToList());
            Rooms = DbContext.Rooms.AsNoTracking().Where(r => r.IsDeleted == false).ToList();
            EquipList = DbContext.Equips
                .Include(i => i.Type)
                .Include(i => i.Status)
                .Include(i => i.MOL)
                .Include(i => i.Room)
                .Where(r => r.IsDeleted == false).ToList();
            Categories = new ObservableCollection<Category>(DbContext.Categories.AsNoTracking().ToList());
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

                    DbContext.Equips
                        .AsNoTracking()
                        .Where(i => i.IsDeleted == false)
                        .ToList().ForEach(i => AllEquip.Add(new EquipInfo
                        {
                            Id = i.Id,
                            RegistrationDate = i.RegistrationDate,
                            Name = i.Name,
                            InvNum = i.InvNum,
                            Org = i.Org,
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
                        var newRoom = DbContext.Rooms.Add(new Room
                            {
                                Name = NewName,
                                OrgId = Nodes.Single().RoomId,
                                IsDeleted = false
                            });

                        DbContext.SaveChanges();
                        
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
                        if(NewName == oldName) return;

                        DbContext.Rooms.Single(r => r.Id == SelectedNode.RoomId).Name = NewName;
                        SelectedNode.Name = NewName;
                        DbContext.SaveChanges();
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
                    DbContext.Types.AsNoTracking().ToList().ForEach(i => EquipTypes.Add(i));
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
                    DbContext.MOLs.AsNoTracking().ToList().ForEach(i => EquipMOLs.Add(i));
                });
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closing event, and cancel here.");
        public ICommand  DeleteRoomCommand
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
                        DbContext.Rooms.Single(r => r.Id == SelectedNode.RoomId).IsDeleted = true;
                        await DbContext.Equips.Where(e => e.RoomId == SelectedNode.RoomId).ForEachAsync(i => i.IsDeleted = true);

                        Nodes[0].Nodes.Remove(SelectedNode);
                        DbContext.SaveChanges();
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
                    if(result is null || !(bool)result) return;

                    var equip = DbContext.Equips
                        .Include(i => i.Type)
                        .Include(i => i.Status)
                        .Include(i => i.MOL)
                        .Include(i => i.Room)
                        .Single(e => e.Id == SelectedEquip.Id);

                    if(equip.BasePrice != SelectedEquip.BasePrice)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.BasePrice,
                            OldValue = equip.BasePrice.ToString("C"),
                            NewValue = SelectedEquip.BasePrice.ToString("C")
                        });
                    if(equip.MOL != SelectedEquip.MOL)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.MOL,
                            OldValue = equip.MOL.ShortFullName.ToString(),
                            NewValue = SelectedEquip.MOL.ShortFullName.ToString()
                        });
                    if(equip.InvNum != SelectedEquip.InvNum)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.InvNum,
                            OldValue = equip.InvNum.ToString(),
                            NewValue = SelectedEquip.InvNum.ToString()
                        });
                    if(equip.RegistrationDate != SelectedEquip.RegistrationDate)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.RegistrationDate,
                            OldValue = equip.RegistrationDate.ToString("MM.dd.yyyy"),
                            NewValue = SelectedEquip.RegistrationDate.ToString("MM.dd.yyyy")
                        });
                    if(equip.BaseInvNum != SelectedEquip.BaseInvNum)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.BaseInvNum,
                            OldValue = equip.BaseInvNum.ToString(),
                            NewValue = SelectedEquip.BaseInvNum.ToString()
                        });
                    if(equip.Type != SelectedEquip.Type)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.Type,
                            OldValue = equip.Type.Name.ToString(),
                            NewValue = SelectedEquip.Type.Name.ToString()
                        });
                    if(equip.Status != SelectedEquip.Status)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.Status,
                            OldValue = equip.Status.Name.ToString(),
                            NewValue = SelectedEquip.Status.Name.ToString()
                        });
                    if(equip.DepreciationGroup != SelectedEquip.DepreciationGroup)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.Depreciation,
                            OldValue = equip.DepreciationGroup.ToString(),
                            NewValue = SelectedEquip.DepreciationGroup.ToString()
                        });
                    if(equip.DepreciationRate != SelectedEquip.DepreciationRate)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.Depreciation,
                            OldValue = equip.DepreciationRate.ToString(),
                            NewValue = SelectedEquip.DepreciationRate.ToString()
                        });
                    if(equip.Name != SelectedEquip.Name)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.Name,
                            OldValue = equip.Name.ToString(),
                            NewValue = SelectedEquip.Name.ToString()
                        });
                    if(equip.Note != SelectedEquip.Note)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.Note,
                            OldValue = equip.Note.ToString(),
                            NewValue = SelectedEquip.Note.ToString()
                        });
                    if(equip.ReleaseDate != SelectedEquip.ReleaseDate)
                        DbContext.History.Add(new History
                        {
                            EquipId = equip.Id,
                            Date = DateTime.Now,
                            Code = (History.OperationCode) 1,
                            ChangedProperty = History.Property.ReleaseDate,
                            OldValue = equip.ReleaseDate.ToString("MM.dd.yyyy"),
                            NewValue = SelectedEquip.ReleaseDate.ToString("MM.dd.yyyy")
                        });

                    equip = SelectedEquip;

                    
                    DbContext.SaveChanges();
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
                        DbContext.Equips.Single(i => i.Id == SelectedEquip.Id).IsDeleted = true;
                        DbContext.History.Add(new History
                        {
                            EquipId = SelectedEquip.Id,
                            Date = DateTime.Now,
                            Code = History.OperationCode.Deleted,
                            ChangedProperty = History.Property.None,
                            OldValue = SelectedEquip.Name,
                            NewValue = "Имущество удалено"
                        });
                        CurrentRoomEquip.Remove(SelectedEquip);
                        DbContext.SaveChanges();
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
                    if(result is null || !(bool)result) return;

                    NewEquip.RoomId = SelectedNode.RoomId;
                    var molId = NewEquip.MOL.Id;
                    NewEquip.MOL = null;
                    NewEquip.MOLId = molId;
                    DbContext.Equips.Add(NewEquip);
                    DbContext.SaveChanges();
                    var id = DbContext.Equips.ToList().Last().Id;
                    DbContext.History.Add(new History
                    {
                        EquipId = id,
                        Date = DateTime.Now,
                        Code = (History.OperationCode) 0,
                        ChangedProperty = History.Property.None,
                        OldValue = "-",
                        NewValue = "Добавлено"
                    });
                    DbContext.SaveChanges();
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
