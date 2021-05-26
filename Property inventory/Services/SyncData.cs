using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Models;
using Type = Property_inventory.Entities.Type;

namespace Property_inventory.Services
{
    public class SyncData
    {
        private string ServerAddress => "http://drdgus.space";
        private string Login => "Android";
        private string Password => "Android";
        private  HttpClient Client { get; set; }


        public SyncData()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                            $"{Login}:{Password}")));

            //HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            Client.Timeout = new TimeSpan(0, 0, 5);

            LoadDb();
        }

        private void LoadDb()
        {
            try
            {
                var db = InvDbContext.GetInstance();
                if (db.Orgs.Any()) return;
                var response = Client.GetAsync(ServerAddress+ "/All");
                response.Wait();

                var allTables = JsonConvert.DeserializeObject<AllTables>(response.Result.Content.ReadAsStringAsync().Result);

                foreach (var VARIABLE in allTables.Accountabilities)
                {
                    db.Database.BeginTransaction();
                    db.Accountabilities.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.Categories)
                {
                    db.Database.BeginTransaction();
                    db.Categories.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.Types)
                {
                    db.Database.BeginTransaction();
                    db.Types.Add(new Type
                    {
                        Name = VARIABLE.Name,
                        CategoryId = VARIABLE.CategoryId,
                    });
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.Rooms)
                {
                    db.Database.BeginTransaction();
                    db.Rooms.Add(new Room
                    {
                        OrgId = VARIABLE.OrgId,
                        Name = VARIABLE.Name,
                        IsDeleted = VARIABLE.IsDeleted
                    });
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.MOLs)
                {
                    db.Database.BeginTransaction();
                    db.MOLs.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }

                db.SaveChanges();
                foreach (var VARIABLE in allTables.Equips)
                {
                    db.Database.BeginTransaction();
                    db.Equips.Add(new Equip
                    {
                        RegistrationDate = VARIABLE.RegistrationDate,
                        Name = VARIABLE.Name,
                        InvNum = VARIABLE.InvNum,
                        OrgId = VARIABLE.OrgId,
                        RoomId = VARIABLE.RoomId,
                        TypeId = VARIABLE.TypeId,
                        StatusId = VARIABLE.StatusId,
                        AccountabilityId = VARIABLE.AccountabilityId,
                        Note = VARIABLE.Note,
                        Count = VARIABLE.Count,
                        IsDeleted = VARIABLE.IsDeleted,
                        MOLId = VARIABLE.MOLId,
                        ReleaseDate = VARIABLE.ReleaseDate,
                        BasePrice = VARIABLE.BasePrice,
                        DepreciationRate = VARIABLE.DepreciationRate,
                        DepreciationGroup = VARIABLE.DepreciationGroup,
                        BaseInvNum = VARIABLE.BaseInvNum
                    });
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.History)
                {
                    db.Database.BeginTransaction();
                    db.History.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.Orgs)
                {
                    db.Database.BeginTransaction();
                    db.Orgs.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.Statuses)
                {
                    db.Database.BeginTransaction();
                    db.Statuses.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }
               

                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw new Exception("Ошибка при получении всех таблиц.");
            }

            
        }

        private void StartListen()
        {

        }

        private void StopListen()
        {

        }
    }

}
