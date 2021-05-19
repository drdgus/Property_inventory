using System;
using System.Collections.Generic;

namespace Property_inventory.Models
{
    public class Equip
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Name { get; set; }
        public string InvNum { get; set; }
        public Org Org { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public Type Type { get; set; }
        public Status Status { get; set; }
        public Accountability Accountability { get; set; }
        public List<History> History { get; set; }
        public string Note { get; set; }
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
    }
}
