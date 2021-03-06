using Property_inventory.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Property_inventory.DAL.Repositories
{
    public class RoomRepository
    {
        public List<Room> Get()
        {
            return InvDbContext.GetInstance().Rooms.AsNoTracking().Where(r => r.IsDeleted == false).ToList();
        }

        public int Add(Room newRoom)
        {
            newRoom.OrgId = 1;
            var room = InvDbContext.GetInstance().Rooms.Add(newRoom);
            InvDbContext.GetInstance().SaveChanges();
            newRoom.Id = room.Id;
            //new SyncData().AddRoom(newRoom);
            return room.Id;
        }

        public void Update(Room room)
        {
            var currnetRoom = InvDbContext.GetInstance().Rooms.Single(i => i.Id == room.Id);
            currnetRoom = room;
            InvDbContext.GetInstance().SaveChanges();
            //new SyncData().UpdateRoom(room);
        }

        public async void Remove(int roomId)
        {
            InvDbContext.GetInstance().Rooms.Single(i => i.Id == roomId).IsDeleted = true;
            await InvDbContext.GetInstance().Equips.Where(e => e.RoomId == roomId)
                .ForEachAsync(i => i.IsWriteOff = true);
            InvDbContext.GetInstance().SaveChanges();
            //new SyncData().RemoveRoom(roomId);
        }
    }
}
