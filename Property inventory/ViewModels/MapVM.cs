using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.DAL.Repositories;
using Property_inventory.Entities;
using Property_inventory.Models;

namespace Property_inventory.ViewModels
{
    public class MapVM : INotifyPropertyChanged
    {
        private System.Windows.Point _mousePosition;

        public MapVM()
        {
            Markers = new List<Views.UserControls.Marker>();

            var rooms = new RoomRepository().Get();
            var equip = new EquipRepository().GetEquip();

            foreach (var room in rooms)
            {
                Markers.Add(new Views.UserControls.Marker());
                Markers.Last().MarkerContent = new Marker()
                {
                    Top = 230,
                    Left = 500,
                    Room = room,
                    Equip = equip.Where(i => i.RoomId == room.Id).ToList()
                };
            }
        }

        public List<Views.UserControls.Marker> Markers { get; set; }

        public System.Windows.Point MousePosition
        {
            get => _mousePosition;
            set
            {
                _mousePosition = value;
                Markers.ForEach(i => i.MarkerContent.MousePosition = MousePosition);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
