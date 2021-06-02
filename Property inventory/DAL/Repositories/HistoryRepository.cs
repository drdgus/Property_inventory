using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.Entities;
using Property_inventory.Services;

namespace Property_inventory.DAL.Repositories
{
    public class HistoryRepository
    {
        public void Add(History history)
        {
            InvDbContext.GetInstance().History.Add(history);
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().AddHistory(history);
        }

        public List<History> Get()
        {
            return InvDbContext.GetInstance().History.AsNoTracking().ToList();
        }
    }
}
