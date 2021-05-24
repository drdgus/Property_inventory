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
                    InvNum = DbContext.Equips.AsNoTracking().Max(i => i.InvNum) + 1,
                    Org = DbContext.Orgs.Single(),
                    RoomId = 0,
                    Status = DbContext.Statuses.First(),
                    Accountability = DbContext.Accountabilities.First(),
                    History = new List<History>(),
                    Note = "",
                    Count = 0,
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


        public ObservableCollection<Equip> CurrentRoomEquip { get; set; }
        public ObservableCollection<Type> EquipTypes { get; set; }
        public ObservableCollection<string> DepreciationGroups { get; set; }
        public ObservableCollection<MOL> EquipMOLs { get; set; }
        public string InvSymbol { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
            }
        }

        public MainVM()
        {
            new SyncData();
            DbContext = InvDbContext.GetInstance();
            //View = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(Nodes);
            //View.IsLiveSorting = true;
            CurrentRoomEquip = new ObservableCollection<Equip>();
            EquipTypes = new ObservableCollection<Type>(DbContext.Types.AsNoTracking().ToList());
            DepreciationGroups = new ObservableCollection<string>(Enum.GetNames(typeof(Equip.DepreciationGroups)));
            EquipMOLs = new ObservableCollection<MOL>(DbContext.MOLs.AsNoTracking().ToList());
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
        public ICommand CreateRoomCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Dialog
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
                    
                });
            }
        }
        public ICommand EditEquipCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    
                });
            }
        }
        public ICommand DeleteEquipCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    
                });
            }
        }
        public ICommand OpenInvCardWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    
                });
            }
        }
        public ICommand OpenRelocateWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    
                });
            }
        }
        public ICommand OpenWriteOffWinCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    
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
                        var view = new InfoDialog()
                        {
                            DataContext = this
                        };
                        MessageDialogContent = "Выберите помещение.";

                        await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
                        return;
                    }

                    NewEquip.RoomId = SelectedNode.RoomId;
                    DbContext.Equips.Add(NewEquip);
                    DbContext.SaveChanges();
                    CurrentRoomEquip.Add(NewEquip);
                    NewEquip = null;
                    ClearCreateEquipCommand.Execute(null);
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
