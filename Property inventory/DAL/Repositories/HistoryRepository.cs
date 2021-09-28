using Property_inventory.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Property_inventory.DAL.Repositories
{
    public class HistoryRepository
    {
        public void Add(History history)
        {
            InvDbContext.GetInstance().History.Add(history);
            InvDbContext.GetInstance().SaveChanges();
            //new SyncData().AddHistory(history);
        }

        public List<History> Get(int id = 0)
        {
            if (id != 0) return InvDbContext.GetInstance().History.AsNoTracking().Where(i => i.ObjectId == id).ToList();
            return InvDbContext.GetInstance().History.AsNoTracking().ToList();
        }
    }
}
