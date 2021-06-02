using Property_inventory.Services;
using System;
using System.Collections.Generic;

namespace Property_inventory.Entities
{
    public class InvDocument
    {
        public int Id { get; set; }
        public List<Equip> Equip { get; set; }
        public InvEnums.DocumentType DocumentType { get; set; }
        public int DocumentNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string URL { get; set; }
    }
}
