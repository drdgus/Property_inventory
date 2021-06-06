using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.Entities;

namespace Property_inventory.DAL.Repositories
{
    public class ManufacturerRepository
    {
        public bool Contains(Manufacturer manufacturer)
        {
            return InvDbContext.GetInstance().Manufacturers.SingleOrDefault(i => i.SupplyerAddress == manufacturer.SupplyerAddress) != null;
        }
    }
}
