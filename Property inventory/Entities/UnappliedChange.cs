using Property_inventory.Services;
using System;

namespace Property_inventory.Entities
{
    public class UnappliedChange
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public InvEnums.Table TableCode { get; set; }
        public object ChangedObject { get; set; }
        public InvEnums.OperationCode OperationType { get; set; }

        public override string ToString()
        {
            return $"CreatedTime: {CreatedTime.ToShortDateString()} Таблица: {TableCode} Объект: {ChangedObject} Операция: {OperationType}";
        }
    }
}
