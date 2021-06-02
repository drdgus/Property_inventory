using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.Entities;
using Property_inventory.Services;
using Type = Property_inventory.Entities.Type;

namespace Property_inventory.DAL.Repositories
{
    public class DictionaryRepository
    {
        public List<Category> GetCategories()
        {
            return InvDbContext.GetInstance().Categories.AsNoTracking().ToList();
        }

        public List<Type> GetTypes()
        {
            return InvDbContext.GetInstance().Types.ToList();
        }

        public List<MOL> GetMOLs()
        {
            return InvDbContext.GetInstance().MOLs.AsNoTracking().ToList();
        }

        public Type AddType(Type type)
        {
            var newType = InvDbContext.GetInstance().Types.Add(type);
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().AddType(type);
            return newType;
        }

        public MOL AddMOL(MOL mol)
        {
            var newMOL = InvDbContext.GetInstance().MOLs.Add(mol);
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().AddMOL(mol);
            return newMOL;
        }

        public void RemoveType(Type selectedItem)
        {
            InvDbContext.GetInstance().Entry(selectedItem).State = EntityState.Deleted;
            InvDbContext.GetInstance().Types.Remove(selectedItem);
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().RemoveType(selectedItem.Id);
        }

        public void RemoveMOL(MOL selectedItem)
        {
            InvDbContext.GetInstance().Entry(selectedItem).State = EntityState.Deleted;
            InvDbContext.GetInstance().MOLs.Remove(selectedItem);
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().RemoveMOL(selectedItem.Id);
        }

        public void UpdateType(Type selectedItem)
        {
            var type = InvDbContext.GetInstance().Types.Single(i => i.Id == selectedItem.Id);
            type = selectedItem;
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().UpdateType(selectedItem);
        }

        public void UpdateMOL(MOL selectedItem)
        {
            var mol = InvDbContext.GetInstance().MOLs.Single(i => i.Id == selectedItem.Id);
            mol = selectedItem;
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().UpdateMOL(selectedItem);
        }
    }
}
