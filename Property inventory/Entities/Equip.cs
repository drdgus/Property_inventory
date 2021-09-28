using Property_inventory.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Runtime.CompilerServices;

namespace Property_inventory.Entities
{
    public class Equip
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Name { get; set; }
        public int InvNum { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public int InvTypeId { get; set; }
        public virtual InvType InvType { get; set; }
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }
        public int AccountabilityId { get; set; }
        public virtual Accountability Accountability { get; set; }
        public virtual List<History> History { get; set; }
        public string Note { get; set; }
        public int Count { get; set; }
        [DefaultValue(false)] public bool IsWriteOff { get; set; }
        public int MOLId { get; set; }
        /// <summary>
        /// Материально ответственное лицо.
        /// </summary>
        public virtual MOL MOL { get; set; }
        /// <summary>
        /// Дата выпуска оборудования
        /// </summary>
        //public DateTime ReleaseDate { get; set; }
        ///// <summary>
        ///// Базовая цена
        ///// </summary>
        //public Decimal BasePrice { get; set; }
        ///// <summary>
        ///// В процентах
        ///// </summary>
        //public int DepreciationRate { get; set; }

        //public InvEnums.DepreciationGroups DepreciationGroup { get; set; }

        //public string BaseInvNum { get; set; }
        ///// <summary>
        ///// В процентах
        ///// </summary>

        //public int ManufacturerId { get; set; }
        //public virtual Manufacturer Manufacturer { get; set; }
    }
}
