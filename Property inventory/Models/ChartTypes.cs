using Property_inventory.Entities;

namespace Property_inventory.Models
{
    public class ChartTypes
    {
        public decimal Sum { get; set; }
        public InvType Type { get; set; }
        public string Category { get; set; }
        public int Count { get; set; }
    }
}
