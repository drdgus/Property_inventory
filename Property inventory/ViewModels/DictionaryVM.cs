using MaterialDesignThemes.Wpf;
using Property_inventory.DAL;
using Property_inventory.DAL.Repositories;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.ViewModels.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using InvType = Property_inventory.Entities.InvType;

namespace Property_inventory.ViewModels
{
    public class DictionaryVM : INotifyPropertyChanged
    {
        private string _messageDialogContent;

        public ObservableCollection<InvType> TypeList { get; set; }
        public ObservableCollection<MOL> MOLList { get; set; }
        public ObservableCollection<MOLPosition> Positions { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public object SelectedItem { get; set; }
        public string NewName { get; set; }
        public string TypeName { get; set; }
        public string FullName { get; set; }
        public int PersonnelNumber { get; set; }
        public string MessageDialogContent
        {
            get => _messageDialogContent;
            set
            {
                _messageDialogContent = value;
                OnPropertyChanged();
            }
        }

        public DictionaryVM()
        {
            TypeList = new ObservableCollection<InvType>(new DictionaryRepository().GetTypes());
            MOLList = new ObservableCollection<MOL>(new DictionaryRepository().GetMOLs());
            Positions = new ObservableCollection<MOLPosition>(new DictionaryRepository().GetMolPositions());
            Categories = new ObservableCollection<Category>(new DictionaryRepository().GetCategories());
        }

        public ICommand CreateTypeCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new CreateTypeUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        var newType = new DictionaryRepository().AddType(new InvType
                        {
                            CategoryId = ((Category)SelectedItem).Id,
                            Name = TypeName,
                        });
                        TypeList.Add(newType);
                    }
                });
            }
        }

        public ICommand CreateMOLCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new CreateMolUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        var newMOL = new DictionaryRepository().AddMOL(new MOL
                        {
                            FullName = FullName,
                            PersonnelNumber = PersonnelNumber,
                            PositionId = ((MOLPosition)SelectedItem).Id
                        });

                        MOLList.Add(newMOL);
                        InvDbContext.GetInstance().SaveChanges();
                    }
                });
            }
        }

        public ICommand DeleteTypeCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new DeleteDialog
                    {
                        DataContext = this
                    };
                    MessageDialogContent = "Удалить выбранный тип?";

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        new DictionaryRepository().RemoveType((InvType)SelectedItem);
                        TypeList.Remove((InvType)SelectedItem);
                    }
                });
            }
        }

        public ICommand DeleteMOLCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new DeleteDialog
                    {
                        DataContext = this
                    };
                    MessageDialogContent = "Удалить выбранное материально ответственное лицо?";

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        new DictionaryRepository().RemoveMOL((MOL)SelectedItem);
                        MOLList.Remove((MOL)SelectedItem);
                    }
                });
            }
        }

        public ICommand EditTypeCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new RenameUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        ((InvType)SelectedItem).Name = NewName;
                        new DictionaryRepository().UpdateType((InvType)SelectedItem);
                    }
                });
            }
        }

        public ICommand EditMOLCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new RenameUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        ((MOL)SelectedItem).FullName = NewName;
                        new DictionaryRepository().UpdateMOL((MOL)SelectedItem);
                    }
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
