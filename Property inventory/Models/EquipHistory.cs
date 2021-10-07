using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Properties;
using System.Linq;
using Property_inventory.Services.Tools;

namespace Property_inventory.Models
{
    public class EquipHistory : History
    {
        private Equip Equip => InvDbContext.GetInstance().Equips.AsNoTracking().Single(i => i.Id == ObjectId);
        public string InvNum => Equip.InvNum;

        public string ChangedPropertyStr => ChangedProperty.GetStringValue();
        public string Name => Equip.Name;
        public string Splitter => "->";

        public EquipHistory()
        {

        }
    }
}
