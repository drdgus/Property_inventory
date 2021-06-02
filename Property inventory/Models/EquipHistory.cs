﻿using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Properties;
using System.Linq;

namespace Property_inventory.Models
{
    public class EquipHistory : History
    {
        private Equip Equip => InvDbContext.GetInstance().Equips.AsNoTracking().Single(i => i.Id == EquipId);
        public string InvNum => Equip.InvNum.ToString($"{Settings.Default.InvSymbol}-0000000");

        public string Name => Equip.Name;

        public EquipHistory()
        {

        }
    }
}
