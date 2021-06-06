using Property_inventory.Services;
using System;

namespace Property_inventory.Entities
{
    public class History
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public InvEnums.Table TableCode { get; set; }
        public DateTime Date { get; set; }
        public InvEnums.OperationCode Code { get; set; }
        public InvEnums.HistoryProperty ChangedProperty { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
