using Property_inventory.Services.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Property_inventory.Entities
{
    public class Equip : IDataErrorInfo, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Name { get; set; }
        public int InvNum { get; set; }
        public int OrgId { get; set; }
        public Org Org { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int TypeId { get; set; }
        public Type Type { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int AccountabilityId { get; set; }
        public Accountability Accountability { get; set; }
        public int HistoryId { get; set; }
        public List<History> History { get; set; }
        public string Note { get; set; }
        public int Count { get; set; }
        [DefaultValue(false)] public bool IsDeleted { get; set; }
        public int MOLId { get; set; }
        /// <summary>
        /// Материально ответственное лицо.
        /// </summary>
        public MOL MOL { get; set; }
        /// <summary>
        /// Дата выпуска оборудования
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Базовая цена
        /// </summary>
        public Decimal BasePrice { get; set; }
        /// <summary>
        /// В процентах
        /// </summary>
        public int DepreciationRate { get; set; }

        public DepreciationGroups DepreciationGroup
        {
            get => _depreciationGroup;
            set
            {
                _depreciationGroup = value;
                OnPropertyChanged();
            }
        }

        public string BaseInvNum { get; set; }
        /// <summary>
        /// В процентах
        /// </summary>

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

        Dictionary<string, string> _errors;
        private Type _type;
        private DepreciationGroups _depreciationGroup;
        public string this[string columnName] => _errors.ContainsKey(columnName) ? _errors[columnName] : null;
        public string Error { get; }
        public bool IsValid => _errors.Values.All(x => x == null);
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
