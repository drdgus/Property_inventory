using Property_inventory.Entities;
using Property_inventory.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Property_inventory.DAL.Repositories
{
    public class EquipRepository
    {
        public List<Equip> GetEquipByRoom(int selectedNodeRoomId, string filter = "")
        {
            var a = InvDbContext.GetInstance().Equips
                .Where(i => i.RoomId == selectedNodeRoomId && i.IsDeleted == false)
                .ToList();
            return a;
        }

        public List<Equip> GetEquip()
        {
            var a = InvDbContext.GetInstance().Equips
                //Исправил добавлением Virtual для навигационных свойств.
                .Where(r => r.IsDeleted == false).ToList();
            return a;
        }
        public Equip GetEquip(int equipId)
        {
            return InvDbContext.GetInstance().Equips
                .Single(e => e.Id == equipId);
        }

        public void Update(Equip selectedEquip)
        {
            var equip = GetEquip(selectedEquip.Id);
            //if (equip.BasePrice != selectedEquip.BasePrice)
            //    new HistoryRepository().Add(new History
            //    {
            //        ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
            //        Date = DateTime.Now,
            //        Code = (InvEnums.OperationCode)1,
            //        ChangedProperty = InvEnums.HistoryProperty.BasePrice,
            //        OldValue = equip.BasePrice.ToString("C"),
            //        NewValue = selectedEquip.BasePrice.ToString("C")
            //    });
            if (equip.MOL != selectedEquip.MOL)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id,
                    TableCode = InvEnums.Table.Equip,
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.MOL,
                    OldValue = equip.MOL.ShortFullName.ToString(),
                    NewValue = selectedEquip.MOL.ShortFullName.ToString()
                });
            if (equip.InvNum != selectedEquip.InvNum)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id,
                    TableCode = InvEnums.Table.Equip,
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.InvNum,
                    OldValue = equip.InvNum.ToString(),
                    NewValue = selectedEquip.InvNum.ToString()
                });
            //if (equip.RegistrationDate != selectedEquip.RegistrationDate)
            //    new HistoryRepository().Add(new History
            //    {
            //        ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
            //        Date = DateTime.Now,
            //        Code = (InvEnums.OperationCode)1,
            //        ChangedProperty = InvEnums.HistoryProperty.RegistrationDate,
            //        OldValue = equip.RegistrationDate.ToString("MM.dd.yyyy"),
            //        NewValue = selectedEquip.RegistrationDate.ToString("MM.dd.yyyy")
            //    });
            //if (equip.BaseInvNum != selectedEquip.BaseInvNum)
            //    new HistoryRepository().Add(new History
            //    {
            //        ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
            //        Date = DateTime.Now,
            //        Code = (InvEnums.OperationCode)1,
            //        ChangedProperty = InvEnums.HistoryProperty.BaseInvNum,
            //        OldValue = equip.BaseInvNum.ToString(),
            //        NewValue = selectedEquip.BaseInvNum.ToString()
            //    });
            if (equip.Type != selectedEquip.Type)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id,
                    TableCode = InvEnums.Table.Equip,
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Type,
                    OldValue = equip.Type.Name.ToString(),
                    NewValue = selectedEquip.Type.Name.ToString()
                });
            if (equip.Status != selectedEquip.Status)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id,
                    TableCode = InvEnums.Table.Equip,
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Status,
                    OldValue = equip.Status.Name.ToString(),
                    NewValue = selectedEquip.Status.Name.ToString()
                });
            //if (equip.DepreciationGroup != selectedEquip.DepreciationGroup)
            //    new HistoryRepository().Add(new History
            //    {
            //        ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
            //        Date = DateTime.Now,
            //        Code = (InvEnums.OperationCode)1,
            //        ChangedProperty = InvEnums.HistoryProperty.DepreciationGroup,
            //        OldValue = equip.DepreciationGroup.ToString(),
            //        NewValue = selectedEquip.DepreciationGroup.ToString()
            //    });
            //if (equip.DepreciationRate != selectedEquip.DepreciationRate)
            //    new HistoryRepository().Add(new History
            //    {
            //        ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
            //        Date = DateTime.Now,
            //        Code = (InvEnums.OperationCode)1,
            //        ChangedProperty = InvEnums.HistoryProperty.DepreciationRate,
            //        OldValue = equip.DepreciationRate.ToString(),
            //        NewValue = selectedEquip.DepreciationRate.ToString()
            //    });
            if (equip.Name != selectedEquip.Name)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id,
                    TableCode = InvEnums.Table.Equip,
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Name,
                    OldValue = equip.Name.ToString(),
                    NewValue = selectedEquip.Name.ToString()
                });
            if (equip.Note != selectedEquip.Note)
                new HistoryRepository().Add(new History
                {
                    ObjectId = equip.Id,
                    TableCode = InvEnums.Table.Equip,
                    Date = DateTime.Now,
                    Code = (InvEnums.OperationCode)1,
                    ChangedProperty = InvEnums.HistoryProperty.Note,
                    OldValue = equip.Note.ToString(),
                    NewValue = selectedEquip.Note.ToString()
                });
            //if (equip.ReleaseDate != selectedEquip.ReleaseDate)
            //    new HistoryRepository().Add(new History
            //    {
            //        ObjectId = equip.Id, TableCode = InvEnums.Table.Equip, 
            //        Date = DateTime.Now,
            //        Code = (InvEnums.OperationCode) 1,
            //        ChangedProperty = InvEnums.HistoryProperty.ReleaseDate,
            //        OldValue = equip.ReleaseDate.ToString("MM.dd.yyyy"),
            //        NewValue = selectedEquip.ReleaseDate.ToString("MM.dd.yyyy")
            //    });

            equip = selectedEquip;

            InvDbContext.GetInstance().SaveChanges();

            //new SyncData().UpdateEquip(equip);
        }

        public void Remove(Equip selectedEquip)
        {
            InvDbContext.GetInstance().Equips.Single(i => i.Id == selectedEquip.Id).IsDeleted = true;
            new HistoryRepository().Add(new History
            {
                ObjectId = selectedEquip.Id,
                TableCode = InvEnums.Table.Equip,
                Date = DateTime.Now,
                Code = InvEnums.OperationCode.Deleted,
                ChangedProperty = InvEnums.HistoryProperty.None,
                OldValue = selectedEquip.Name,
                NewValue = "Имущество удалено"
            });
            InvDbContext.GetInstance().SaveChanges();
            //new SyncData().RemoveEquip(selectedEquip.Id);
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
                IsDeleted = false,
                MOLId = newEquip.MOLId,
                //ReleaseDate = newEquip.ReleaseDate,
                //BasePrice = newEquip.BasePrice,
                //DepreciationRate = newEquip.DepreciationRate,
                //DepreciationGroup = newEquip.DepreciationGroup,
                //BaseInvNum = newEquip.BaseInvNum,
                //Manufacturer = newEquip.Manufacturer,
            };

            var addedEquip = InvDbContext.GetInstance().Equips.Add(newEquip);
            InvDbContext.GetInstance().SaveChanges();
            new HistoryRepository().Add(new History
            {
                ObjectId = addedEquip.Id,
                TableCode = InvEnums.Table.Equip,
                Date = DateTime.Now,
                Code = InvEnums.OperationCode.Created,
                ChangedProperty = InvEnums.HistoryProperty.None,
                OldValue = "-",
                NewValue = "На балансе"
            });
            //new SyncData().AddEquip(equip);

            return addedEquip;
        }
    }
}
