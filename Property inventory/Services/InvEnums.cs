using Property_inventory.Services.Tools;

namespace Property_inventory.Services
{
    public class InvEnums
    {
        public enum OperationCode
        {
            Created,
            Edited,
            Deleted,
            Supply,
            OnBalance,
            Relocate,
            WriteOff
        }

        public enum Table
        {
            Equip,
            Room,
            Org,
            Type,
            Status,
            Accountability,
            CheckInfo,
            MOL
        }

        public enum DocumentType
        {
            Supply,
            Balance,
            InvCard,
            Relocate,
            Handover,
            HandoverToMOL,
            HandoverToNewMOL,
            WriteOff,
            AllEquipOC,
            AllEquipTMC,
        }

        public enum HistoryProperty
        {
            [StringValue("-")] None,
            [StringValue("Название")] Name,
            [StringValue("Инвентарный номер")] InvNum,
            [StringValue("Помещение")] Room,
            [StringValue("Тип")] Type,
            [StringValue("Статус")] Status,
            [StringValue("Подотчет")] Accountability,
            [StringValue("Заметка")] Note,
            [StringValue("Количество")] Count,
            [StringValue("Дата производства")] ReleaseDate,
            [StringValue("Производитель")] Manufacturer,
            [StringValue("Заводской инвентарный номер")] BaseInvNum,
            [StringValue("Группа амортизации")] DepreciationGroup,
            [StringValue("% амортизационных начислений")] DepreciationRate,
            [StringValue("Первоначальная стоимость")] BasePrice,
            [StringValue("МОЛ")] MOL,
            [StringValue("Дата регистрации")] RegistrationDate
        }

        public enum DepreciationGroups
        {
            [StringValue("Первая группа")] I,
            [StringValue("Вторая группа")] II,
            [StringValue("Третья группа")] III,
            [StringValue("Четвертая группа")] IV,
            [StringValue("Пятая группа")] V,
            [StringValue("Шестая группа")] VI,
            [StringValue("Седьмая группа")] VII,
            [StringValue("Восьмая группа")] VIII
        }
    }
}
