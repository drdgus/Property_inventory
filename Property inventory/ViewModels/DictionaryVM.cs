using MaterialDesignThemes.Wpf;
using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.ViewModels.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Type = Property_inventory.Entities.Type;

namespace Property_inventory.ViewModels
{
    public class DictionaryVM : INotifyPropertyChanged
    {
        private string _messageDialogContent;

        public ObservableCollection<Type> TypeList { get; set; }
        public ObservableCollection<MOL> MOLList { get; set; }
        public object SelectedItem { get; set; }
        public string NewName { get; set; }
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
            TypeList = new ObservableCollection<Type>(InvDbContext.GetInstance().Types.AsNoTracking().ToList());
            MOLList = new ObservableCollection<MOL>(InvDbContext.GetInstance().MOLs.AsNoTracking().ToList());
        }

        public ICommand CreateTypeCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new CreateRoomUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        var newType = InvDbContext.GetInstance().Types.Add(new Type
                        {
                            Name = NewName,
                        });
                        TypeList.Add(newType);
                        InvDbContext.GetInstance().SaveChanges();
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
                    var view = new CreateRoomUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        var newMOL = InvDbContext.GetInstance().MOLs.Add(new MOL
                        {
                            FullName = NewName
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
                        InvDbContext.GetInstance().Entry(SelectedItem).State = EntityState.Deleted;
                        InvDbContext.GetInstance().Types.Remove((Type)SelectedItem);
                        InvDbContext.GetInstance().SaveChanges();

                        TypeList.Remove((Type)SelectedItem);
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
                        InvDbContext.GetInstance().Entry(SelectedItem).State = EntityState.Deleted;
                        InvDbContext.GetInstance().MOLs.Remove((MOL)SelectedItem);
                        InvDbContext.GetInstance().SaveChanges();

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
                        InvDbContext.GetInstance().Types.Single(r => r.Id == ((Type)SelectedItem).Id).Name = NewName;
                        InvDbContext.GetInstance().SaveChanges();
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
                        InvDbContext.GetInstance().MOLs.Single(r => r.Id == ((MOL)SelectedItem).Id).FullName = NewName;
                        InvDbContext.GetInstance().SaveChanges();
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
