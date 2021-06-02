using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.Entities;
using Property_inventory.Services;

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
            var id = InvDbContext.GetInstance().Rooms.Add(newRoom).Id;
            newRoom.Id = id;
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().AddRoom(newRoom);
            return id;
        }

        public void Update(Room room)
        {
            var currnetRoom = InvDbContext.GetInstance().Rooms.Single(i => i.Id == room.Id);
            currnetRoom = room;
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().UpdateRoom(room);
        }

        public async void Remove(int roomId)
        {
            InvDbContext.GetInstance().Rooms.Single(i => i.Id == roomId).IsDeleted = true;
            await InvDbContext.GetInstance().Equips.Where(e => e.RoomId == roomId)
                .ForEachAsync(i => i.IsDeleted = true);
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().RemoveRoom(roomId);
        }
    }
}
