using Property_inventory.Entities;
using System.Collections.Generic;

namespace Property_inventory.Models
{
    public class AllTables
    {
        public List<Equip> Equips { get; set; }
        public List<Org> Orgs { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Status> Statuses { get; set; }
        public List<Entities.Type> Types { get; set; }
        public List<Accountability> Accountabilities { get; set; }
        public List<History> History { get; set; }
        public List<MOL> MOLs { get; set; }
        public List<Category> Categories { get; set; }
        public List<UnappliedChange> UnappliedChanges { get; set; }
        public List<InvDocument> InvDocuments { get; set; }
        public List<MOLPosition> MolPositions { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
    }
}
