using Newtonsoft.Json;
using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Type = Property_inventory.Entities.Type;

namespace Property_inventory.Services
{
    public class SyncData
    {
        private string ServerAddress => "http://drdgus.space";
        private string Login => "Android";
        private string Password => "Android";
        private HttpClient Client { get; set; }

        HubConnection connection;

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
            StartListen();
            
            try
            {
                var db = InvDbContext.GetInstance();
                if (db.Orgs.Any()) return;
                var response = Client.GetAsync(ServerAddress + "/All");
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
                        BaseInvNum = VARIABLE.BaseInvNum,
                        ManufacturerId = VARIABLE.ManufacturerId
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
                foreach (var VARIABLE in allTables.InvDocuments)
                {
                    db.Database.BeginTransaction();
                    db.InvDocuments.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.Manufacturers)
                {
                    db.Database.BeginTransaction();
                    db.Manufacturers.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }
                foreach (var VARIABLE in allTables.MolPositions)
                {
                    db.Database.BeginTransaction();
                    db.MolPositions.Add(VARIABLE);
                    db.Database.CurrentTransaction.Commit();
                }


                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при получении всех таблиц.");
            }


        }

        private async void SubscribeToChanges()
        {
            await Client.GetAsync(ServerAddress + "/Subscribe");
        }

        private async void StartListen()
        {
            //SubscribeToChanges();
            //IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());

            //IPEndPoint ipPoint = new IPEndPoint(ipHostEntry.AddressList[0], 80);

           
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44304/ChangesHub")
                .WithAutomaticReconnect()
                .Build();

            connection.On<List<UnappliedChange>>("ReceiveChanges", (changesList) =>
            {
                Console.WriteLine(changesList);
            });

            try
            {
                await connection.StartAsync();
                await connection.InvokeAsync("SendChangesList");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void StopListen()
        {

        }


        public void Relocate(int equipId, int newRoomId)
        {
            throw new NotImplementedException();
        }

        #region Add

        public void AddEquip(Equip newEquip)
        {
            throw new NotImplementedException();
        }

        public void AddHistory(History history)
        {
            throw new NotImplementedException();
        }

        public void AddType(Type type)
        {
            throw new NotImplementedException();
        }

        public void AddMOL(MOL mol)
        {
            throw new NotImplementedException();
        }

        public void AddRoom(Room newRoom)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Remove

        public void RemoveType(int selectedItemId)
        {
            throw new NotImplementedException();
        }

        public void RemoveMOL(int selectedItemId)
        {
            throw new NotImplementedException();
        }

        public void RemoveEquip(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoom(int roomId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Update

        public void UpdateType(Type selectedItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateMOL(MOL selectedItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateRoom(Room room)
        {
            throw new NotImplementedException();
        }

        public void UpdateEquip(Equip equip)
        {
            throw new NotImplementedException();
        }

        #endregion
       
    }

}
