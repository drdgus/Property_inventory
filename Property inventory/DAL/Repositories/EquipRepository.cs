using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.Entities;
using Property_inventory.Properties;
using Property_inventory.Services;

namespace Property_inventory.DAL.Repositories
{
    public class EquipRepository
    {
        public List<Equip> GetEquipByRoom(int selectedNodeRoomId, string filter = "")
        {
            return InvDbContext.GetInstance().Equips
                .AsNoTracking()
                .Include(i => i.Type)
                .Include(i =>i.Type.Category)
                .Include(i => i.Status)
                .Include(e => e.MOL)
                .Include(e => e.Type.Category)
                .Where(i => i.RoomId == selectedNodeRoomId && i.IsWriteOff == false)
                .ToList();
        }

        public List<Equip> GetEquip()
        {
            return InvDbContext.GetInstance().Equips
                .AsNoTracking()
                .Include(i => i.Type)
                .Include(i =>i.Type.Category)
                .Include(i => i.Status)
                .Include(i => i.MOL)
                .Include(i => i.MOL.Position)
                .Include(i => i.Room)
                .Where(r => r.IsWriteOff == false).ToList();
        }
        public Equip GetEquip(int equipId)
        {
            return InvDbContext.GetInstance().Equips
                .Include(i => i.Type)
                .Include(i =>i.Type.Category)
                .Include(i => i.Status)
                .Include(i => i.MOL)
                .Include(i => i.Room)
                .Single(e => e.Id == equipId);
        }

        public List<Equip> GetDeletedEquip()
        {
            return InvDbContext.GetInstance().Equips
                .AsNoTracking()
                .Include(i => i.Type)
                .Include(i =>i.Type.Category)
                .Include(i => i.Status)
                .Include(i => i.MOL)
                .Include(i => i.Room)
                .Where(r => r.IsWriteOff == true).ToList();
        }

        public void Update(Equip selectedEquip)
        {
            var equip = GetEquip(selectedEquip.Id);
            if (equip.BasePrice != selectedEquip.BasePrice)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.BasePrice,
                    OldValue = equip.BasePrice.ToString("C"),
                    NewValue = selectedEquip.BasePrice.ToString("C")
                });
            if (equip.MOL != selectedEquip.MOL)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.MOL,
                    OldValue = equip.MOL.ShortFullName.ToString(),
                    NewValue = selectedEquip.MOL.ShortFullName.ToString()
                });
            if (equip.InvNum != selectedEquip.InvNum)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.InvNum,
                    OldValue = equip.InvNum.ToString(),
                    NewValue = selectedEquip.InvNum.ToString()
                });
            if (equip.RegistrationDate != selectedEquip.RegistrationDate)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.RegistrationDate,
                    OldValue = equip.RegistrationDate.ToString("MM.dd.yyyy"),
                    NewValue = selectedEquip.RegistrationDate.ToString("MM.dd.yyyy")
                });
            if (equip.BaseInvNum != selectedEquip.BaseInvNum)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.BaseInvNum,
                    OldValue = equip.BaseInvNum.ToString(),
                    NewValue = selectedEquip.BaseInvNum.ToString()
                });
            if (equip.Type != selectedEquip.Type)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Type,
                    OldValue = equip.Type.Name.ToString(),
                    NewValue = selectedEquip.Type.Name.ToString()
                });
            if (equip.Status != selectedEquip.Status)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Status,
                    OldValue = equip.Status.Name.ToString(),
                    NewValue = selectedEquip.Status.Name.ToString()
                });
            if (equip.DepreciationGroup != selectedEquip.DepreciationGroup)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.DepreciationGroup,
                    OldValue = equip.DepreciationGroup.ToString(),
                    NewValue = selectedEquip.DepreciationGroup.ToString()
                });
            if (equip.DepreciationRate != selectedEquip.DepreciationRate)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.DepreciationRate,
                    OldValue = equip.DepreciationRate.ToString(),
                    NewValue = selectedEquip.DepreciationRate.ToString()
                });
            if (equip.Name != selectedEquip.Name)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Name,
                    OldValue = equip.Name.ToString(),
                    NewValue = selectedEquip.Name.ToString()
                });
            if (equip.Note != selectedEquip.Note)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Note,
                    OldValue = equip.Note.ToString(),
                    NewValue = selectedEquip.Note.ToString()
                });
            if (equip.ReleaseDate != selectedEquip.ReleaseDate)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode) 1,
                    ChangedProperty = InvEnums.HistoryProperty.ReleaseDate,
                    OldValue = equip.ReleaseDate.ToString("MM.dd.yyyy"),
                    NewValue = selectedEquip.ReleaseDate.ToString("MM.dd.yyyy")
                });

            equip = selectedEquip;

            InvDbContext.GetInstance().SaveChanges();

            new SyncData().UpdateEquip(equip);
        }

        public void Remove(Equip selectedEquip)
        {
            InvDbContext.GetInstance().Equips.Single(i => i.Id == selectedEquip.Id).IsWriteOff = true;
            new HistoryRepository().Add(new History
            {
                ObjectId = selectedEquip.Id,
                TableCode = InvEnums.Table.Equip,
                Date = DateTime.Now,
                Code = InvEnums.OperationCode.WriteOff,
                ChangedProperty = InvEnums.HistoryProperty.None,
                OldValue = selectedEquip.Name,
                NewValue = "Имущество удалено"
            });
            InvDbContext.GetInstance().SaveChanges();
            new SyncData().RemoveEquip(selectedEquip.Id);
        }

        public Equip Add(Equip newEquip)
        {
            var equip = new Equip
            {
                RegistrationDate = newEquip.RegistrationDate,
                Name = newEquip.Name,
                InvNum = newEquip.InvNum,
                RoomId = newEquip.RoomId,
                TypeId = newEquip.TypeId,
                StatusId = newEquip.StatusId,
                AccountabilityId = newEquip.AccountabilityId,
                Note = newEquip.Note,
                Count = newEquip.Count,
                IsWriteOff = false,
                MOLId = newEquip.MOLId,
                ReleaseDate = newEquip.ReleaseDate,
                BasePrice = newEquip.BasePrice,
                DepreciationRate = newEquip.DepreciationRate,
                DepreciationGroup = newEquip.DepreciationGroup,
                BaseInvNum = newEquip.BaseInvNum,
                Manufacturer = newEquip.Manufacturer,
            };

            var addedEquip = InvDbContext.GetInstance().Equips.Add(newEquip);
            InvDbContext.GetInstance().SaveChanges();
            new HistoryRepository().Add(new History
            {
                ObjectId = addedEquip.Id,
                TableCode = InvEnums.Table.Equip,
                Date = DateTime.Now,
                Code = InvEnums.OperationCode.OnBalance,
                ChangedProperty = InvEnums.HistoryProperty.None,
                OldValue = "-",
                NewValue = "На балансе"
            });

            //var doc = InvDbContext.GetInstance().InvDocuments.Where(i => i.DocumentType == InvEnums.DocumentType.Supply)
            //    .Max(i => i.DocumentNumber) + 1;

            //InvDbContext.GetInstance().InvDocuments.Add(new InvDocument
            //{
            //    Equip = new List<Equip>{addedEquip},
            //    DocumentType = InvEnums.DocumentType.Supply,
            //    DocumentNumber = doc,
            //    CreatedDate = DateTime.Now,
            //    URL = null
            //});
            InvDbContext.GetInstance().SaveChanges();

            new SyncData().AddEquip(equip);

            return addedEquip;
        }

        public void Relocate(Equip equip, Room selectedNewRoom, MOL mol)
        {
            new HistoryRepository().Add(new History
            {
                ObjectId = equip.Id,
                TableCode = InvEnums.Table.Equip,
                Date = DateTime.Now,
                Code = InvEnums.OperationCode.Relocate,
                ChangedProperty = InvEnums.HistoryProperty.Room,
                OldValue = InvDbContext.GetInstance().Rooms.Single(i => i.Id == equip.RoomId).Name,
                NewValue = selectedNewRoom.Name
            });
            InvDbContext.GetInstance().Equips.Single(i => i.Id == equip.Id).RoomId = selectedNewRoom.Id;
            InvDbContext.GetInstance().Equips.Single(i => i.Id == equip.Id).MOLId = mol.Id;
            InvDbContext.GetInstance().SaveChanges();

            new SyncData().Relocate(equip.Id, selectedNewRoom.Id, mol.Id);
        }

        public void Decomission(Equip equip)
        {
            new HistoryRepository().Add(new History
            {
                ObjectId = equip.Id,
                TableCode = InvEnums.Table.Equip,
                Date = DateTime.Now,
                Code = InvEnums.OperationCode.Deleted,
                ChangedProperty = InvEnums.HistoryProperty.None,
                OldValue = "На балансе",
                NewValue = "Списано"
            });
            InvDbContext.GetInstance().Equips.Single(i => i.Id == equip.Id).IsWriteOff = true;
            InvDbContext.GetInstance().SaveChanges();

            new SyncData().Decomission(equip.Id);
        }
    }
}
