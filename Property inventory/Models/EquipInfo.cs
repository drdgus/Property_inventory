using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.Entities;

namespace Property_inventory.Models
{
    public class EquipInfo : Equip
    {
        public int Free { get; set; }
        public int Used { get; set; }
    }
}
