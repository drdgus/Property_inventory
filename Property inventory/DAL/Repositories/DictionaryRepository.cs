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
            return InvDbContext.GetInstance().Types.Include(i => i.Category).ToList();
        }

        public List<MOL> GetMOLs()
        {
            return InvDbContext.GetInstance().MOLs.AsNoTracking().Include(i => i.Position).ToList();
        }

        public List<MOLPosition> GetMolPositions()
        {
            return InvDbContext.GetInstance().MolPositions.AsNoTracking().ToList();
        }

        public Type AddType(Type type)
        {
            var newType = InvDbContext.GetInstance().Types.Add(type);
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().AddType(new Type
            {
                CategoryId = type.CategoryId,
                Name = type.Name
            });
            return newType;
        }

        public MOL AddMOL(MOL mol)
        {
            var newMOL = InvDbContext.GetInstance().MOLs.Add(mol);
            InvDbContext.GetInstance().SaveChanges();
            newMOL = InvDbContext.GetInstance().MOLs.Include(i => i.Position).Single(i => i.Id == newMOL.Id);
            new SyncData().AddMOL(new MOL
            {
                FullName = mol.FullName,
                PositionId = mol.PositionId,
                PersonnelNumber = mol.PersonnelNumber
            });
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
