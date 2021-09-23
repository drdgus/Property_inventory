using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Property_inventory.Models
{
    public class Supply
    {
        public string SupplierName { get; set; }
        public string SupplierAddressPhone { get; set; }
        public string SupplierRequisites { get; set; }

        public string ManufacturerName { get; set; }

        public string TransportName { get; set; }
        public string TransportRequisites { get; set; }

        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public DateTime CheckStart { get; set; }
        public DateTime CheckEnd { get; set; }

        public string EquipName { get; set; }
        public string EquipBaseInvNum { get; set; }
        public decimal EquipBasePrice { get; set; }
        public decimal EquipTotalPrice { get; set; }
    }
}
