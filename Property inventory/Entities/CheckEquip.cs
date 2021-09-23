using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Property_inventory.Entities
{
    public class CheckEquip
    {
        public int Id { get; set; }
        public int CountFact { get; set; }
        public int EquipId { get; set; }
        public Equip Equip { get; set; }
    }
}
