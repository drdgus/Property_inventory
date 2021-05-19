using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Property_inventory.Infrastructure;
using Property_inventory.Models;

namespace Property_inventory.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        private ObservableCollection<Node> _nodes;
        private Node _selectedNode;
        private string _searchText;

        public Node SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;

                EquipList.Clear();
                //EquipList.Add();
            }
        }

        public ObservableCollection<Node> Nodes
        {
            get
            {
                if (_nodes is null)
                {
                    _nodes = new ObservableCollection<Node>
                    {
                        new Node
                        {
                            Name = "МКОУ ТСОШ №20",
                            SortIndex = 0,
                            Nodes = new ObservableCollection<Node>
                            {
                                new Node
                                {
                                    Name = "Каб. 101",
                                    SortIndex = 0
                                },
                                new Node
                                {
                                    Name = "Каб. 102",
                                    SortIndex = 0
                                },
                                new Node
                                {
                                    Name = "Каб. 201",
                                    SortIndex = 0
                                },
                                new Node
                                {
                                    Name = "Каб. 301",
                                    SortIndex = 0,
                                    Nodes = new ObservableCollection<Node>
                                    {
                                        new Node
                                        {
                                            Name = "под кабинет",
                                            SortIndex = 0
                                        }
                                    }
                                }
                            }
                        }
                    };
                }
                return _nodes;
            }
            set => _nodes = value;
        }

        public ObservableCollection<Equip> EquipList { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public MainVM()
        {
            //View = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(Nodes);
            //View.IsLiveSorting = true;
            EquipList = new ObservableCollection<Equip>();
        }

        public ICommand CreateBackupCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    // Сортировка
                    //var list = Nodes[0].Nodes.OrderBy(n => n.Name).ToList();
                    //Nodes[0].Nodes.Clear();
                    //list.ForEach(i => Nodes[0].Nodes.Add(i));
                    //Nodes[0].IsExpanded = true;
                    //Nodes[0].Nodes.Single(n => n.Name == "Каб. 301").IsExpanded = true;
                    //View.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    //Nodes[0].Nodes.Single(n => n.Name == "Каб. 301").Name = "а";
                    //Nodes[0].Nodes.Add(new Node { Name = "а" });
                    //var a = SelectedItem;
                });
            }
        }

        public ICommand SortUpCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //var count = Nodes[0].Nodes.Count();
                    //if ((Nodes[0].Nodes.IndexOf(SelectedNode) - 1) <= 0) return;

                    //var nextNode = Nodes[0].Nodes[Nodes[0].Nodes.IndexOf(SelectedNode) - 1];

                    //SelectedNode.SortIndex = nextNode.SortIndex + 1;
                });
            }
        }

        public ICommand SortDownCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //Использовать SortIndex
                    //var nextNode = Nodes[0].Nodes[Nodes[0].Nodes.IndexOf(SelectedNode) + 1];
                    //if(nextNode is null) return;
                    
                    //SelectedNode.SortIndex = Nodes[0].Nodes[Nodes[0].Nodes.IndexOf(SelectedNode) + 1].SortIndex - 1;
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
