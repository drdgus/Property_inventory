using Property_inventory.Entities;
using System.Linq;

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
