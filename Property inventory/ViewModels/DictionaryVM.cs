using MaterialDesignThemes.Wpf;
using Property_inventory.DAL;
using Property_inventory.DAL.Repositories;
using Property_inventory.Entities;
using Property_inventory.Infrastructure;
using Property_inventory.ViewModels.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using InvType = Property_inventory.Entities.InvType;

namespace Property_inventory.ViewModels
{
    public class DictionaryVM : INotifyPropertyChanged
    {
        private string _messageDialogContent;

        public ObservableCollection<InvType> InvTypes { get; set; }
        public ObservableCollection<MOL> MOLList { get; set; }
        public ObservableCollection<MOLPosition> Positions { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public object SelectedItem { get; set; }
        public Category SelectedCategory { get; set; }
        public int SelectedCategoryIndex { get; set; }
        public Category NewCategory { get; set; }
        public InvType NewInvType { get; set; }
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
            InvTypes = new ObservableCollection<InvType>(new DictionaryRepository().GetTypes());
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
                    NewInvType = new InvType();
                   
                    var view = new CreateTypeUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        if (NewInvType.Category == null) return;
                        if (string.IsNullOrWhiteSpace(NewInvType.Name)) return;

                        var newType = new DictionaryRepository().AddType(new InvType
                        {
                            CategoryId = NewInvType.Category.Id,
                            Name = NewInvType.Name,
                        });
                        InvTypes.Add(newType);
                        NewInvType = null;
                    }
                });
            }
        }

        public ICommand CreateCategoryCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    var view = new CreateCategoryUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        if (string.IsNullOrWhiteSpace(NewCategory.Class)) return;
                        if (string.IsNullOrWhiteSpace(NewCategory.Name)) return;

                        var newCategory = new DictionaryRepository().AddCategory(NewCategory);
                        Categories.Add(newCategory);
                        NewCategory = null;
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
                    if (InvDbContext.GetInstance().Equips.Where(i => i.InvTypeId == ((InvType)SelectedItem).Id).Count() > 0)
                    {
                        var info = new InfoDialog
                        {
                            DataContext = this
                        };
                        MessageDialogContent = "К выбранному типу привязано имущество.";

                        await DialogHost.Show(info, "RootDialogDic");
                        return;
                    }

                    var view = new DeleteDialog
                    {
                        DataContext = this
                    };
                    MessageDialogContent = "Удалить выбранный тип?";

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        new DictionaryRepository().RemoveType((InvType)SelectedItem);
                        InvTypes.Remove((InvType)SelectedItem);
                    }
                });
            }
        }
        public ICommand DeleteCategoryCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    if (InvDbContext.GetInstance().Categories.Where(i => i.Id == ((Category)SelectedItem).Id).Count() > 0)
                    {
                        var info = new InfoDialog
                        {
                            DataContext = this
                        };
                        MessageDialogContent = "К выбранной категории привязан тип.";

                        await DialogHost.Show(info, "RootDialogDic");
                        return;
                    }

                    var view = new DeleteDialog
                    {
                        DataContext = this
                    };
                    MessageDialogContent = "Удалить выбранную категорию?";

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        new DictionaryRepository().RemoveCategory((Category)SelectedItem);
                        Categories.Remove((Category)SelectedItem);
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
                    NewName = ((InvType)SelectedItem).Name;
                    SelectedCategoryIndex = Categories.IndexOf(Categories.Single(i => i.Id == ((InvType)SelectedItem).CategoryId));

                    var view = new EditTypeUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        if (SelectedCategory == null) return;
                        if (string.IsNullOrWhiteSpace(NewName)) return;

                        ((InvType)SelectedItem).Name = NewName;
                        ((InvType)SelectedItem).CategoryId = SelectedCategory.Id;

                        new DictionaryRepository().UpdateType((InvType)SelectedItem);
                    }
                });
            }
        }
        public ICommand EditCategoryCommand
        {
            get
            {
                return new RelayCommand(async o =>
                {
                    NewCategory = SelectedItem as Category;

                    var view = new EditCategoryUC()
                    {
                        DataContext = this
                    };

                    var result = await DialogHost.Show(view, "RootDialogDic");
                    if (result != null && (bool)result)
                    {
                        if (string.IsNullOrWhiteSpace(NewCategory.Class)) return;
                        if (string.IsNullOrWhiteSpace(NewCategory.Name)) return;

                        new DictionaryRepository().UpdateCategory(NewCategory);
                    }

                    NewCategory = null;
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
