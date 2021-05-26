using Property_inventory.Services.Tools;
using System;

namespace Property_inventory.Entities
{
    public class History
    {
        public int Id { get; set; }
        public int EquipId { get; set; }
        public DateTime Date { get; set; }
        public OperationCode Code { get; set; }
        public Property ChangedProperty { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public enum OperationCode
        {
            Created = 1,
            Edited = 2,
            Deleted = 3
        }

        public enum Property
        {
            [StringValue("-")]
            None,
            [StringValue("Название")]
            Name,
            [StringValue("Инвентарный номер")]
            InvNum,
            [StringValue("Помещение")]
            Room,
            [StringValue("Тип")]
            Type,
            [StringValue("Статус")]
            Status,
            [StringValue("Подотчет")]
            Accountability,
            [StringValue("Заметка")]
            Note,
            [StringValue("Количество")]
            Count,
            [StringValue("Базовая стоимость")]
            BasePrice,
            [StringValue("Дата регистрации")]
            RegistrationDate,
            [StringValue("Дата выпуска")]
            ReleaseDate,
            [StringValue("Амортизация")]
            Depreciation,
            [StringValue("Смена МОЛ")]
            MOL,
            [StringValue("Смена заводского инвентарного номера")]
            BaseInvNum
        }
    }
}
