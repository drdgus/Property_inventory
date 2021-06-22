using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Property_inventory.Entities;

namespace Property_inventory.Models
{
    public class Marker : INotifyPropertyChanged
    {
        private double _top;
        private double _left;
        private System.Windows.Point _mousePosition;

        public double Top
        {
            get => _top;
            set
            {
                _top = value;
                OnPropertyChanged();
            }
        }

        public double Left
        {
            get => _left;
            set
            {
                _left = value;
                OnPropertyChanged();
            }
        }

        public System.Windows.Point MousePosition
        {
            get => _mousePosition;
            set
            {
                _mousePosition = value;
                Left = _mousePosition.X;
                Top = _mousePosition.Y;
            }
        }

        public Room Room { get; set; }
        public List<Equip> Equip { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
