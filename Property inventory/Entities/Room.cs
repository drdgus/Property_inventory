﻿namespace Property_inventory.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrgId { get; set; }
        public bool IsDeleted { get; set; }
    }
}