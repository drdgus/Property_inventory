using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Property_inventory.DAL;
using Property_inventory.Entities;
using Property_inventory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using Type = Property_inventory.Entities.Type;

namespace Property_inventory.Services
{
    public class SyncData
    {
        private string ServerAddress => "http://drdgus.space";
        private string TESTServer => "http://localhost";
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
            StartListen();
        }

        private void LoadDb()
        {
            try
            {
                //if(File.Exists("inv.db")) File.Delete("inv.db");
                
                var db = InvDbContext.GetInstance();
                if (db.Orgs.Any()) return;


                var response = Client.GetAsync($"{TESTServer}/All");
                response.Wait();

                var allTables = JsonConvert.DeserializeObject<AllTables>(response.Result.Content.ReadAsStringAsync().Result);
                
                db.Database.BeginTransaction();
                foreach (var accountability in allTables.Accountabilities)
                {
                    db.Accountabilities.Add(accountability);
                }
                foreach (var category in allTables.Categories)
                {
                    db.Categories.Add(category);
                }
                foreach (var type in allTables.Types)
                {
                    db.Types.Add(new Type
                    {
                        Id = type.Id,
                        Name = type.Name,
                        CategoryId = type.CategoryId,
                    });
                }
                foreach (var room in allTables.Rooms)
                {
                    db.Rooms.Add(new Room
                    {
                        Id = room.Id,
                        OrgId = room.OrgId,
                        Name = room.Name,
                        IsDeleted = room.IsDeleted
                    });
                }
                foreach (var MOL in allTables.MOLs)
                {
                    db.MOLs.Add(MOL);
                }
                foreach (var equip in allTables.Equips)
                {
                    db.Equips.Add(new Equip
                    {
                        Id = equip.Id,
                        RegistrationDate = equip.RegistrationDate,
                        Name = equip.Name,
                        InvNum = equip.InvNum,
                        RoomId = equip.RoomId,
                        TypeId = equip.TypeId,
                        StatusId = equip.StatusId,
                        AccountabilityId = equip.AccountabilityId,
                        Note = equip.Note,
                        Count = equip.Count,
                        IsWriteOff = equip.IsWriteOff,
                        MOLId = equip.MOLId,
                        ReleaseDate = equip.ReleaseDate,
                        BasePrice = equip.BasePrice,
                        DepreciationRate = equip.DepreciationRate,
                        DepreciationGroup = equip.DepreciationGroup,
                        BaseInvNum = equip.BaseInvNum,
                        ManufacturerId = equip.ManufacturerId
                    });
                }
                foreach (var history in allTables.History)
                {
                    db.History.Add(history);
                }
                foreach (var org in allTables.Orgs)
                {
                    db.Orgs.Add(org);
                }
                foreach (var status in allTables.Statuses)
                {
                    db.Statuses.Add(status);
                }
                foreach (var invDocument in allTables.InvDocuments)
                {
                    db.InvDocuments.Add(invDocument);
                }
                foreach (var manufacturer in allTables.Manufacturers)
                {
                    db.Manufacturers.Add(manufacturer);
                }
                foreach (var molPosition in allTables.MolPositions)
                {
                    db.MolPositions.Add(molPosition);
                }
                foreach (var check in allTables.CheckEquips)
                {
                    db.CheckEquips.Add(check);
                }

               // db.Database.CurrentTransaction.Commit();
                db.SaveChanges();
            }
            catch(Exception e)
            {
                 throw new Exception($"Ошибка при получении таблиц с сервера. {e}");
            }
        }

        private async void StartListen()
        {
            connection = new HubConnectionBuilder()
                .WithUrl($"{TESTServer}/ChangesHub")
                .WithAutomaticReconnect()
                .Build();

            connection.On<List<UnappliedChange>>("ReceiveChanges", (changesList) =>
            {
                MessageBox.Show($"Изменения: {String.Join("\n",changesList)}");
            });

            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void Relocate(int equipId, int newRoomId, int molId)
        {
            await connection.InvokeAsync("InvRelocate", equipId, newRoomId, molId);
        }

        #region Add

        public async void AddEquip(Equip newEquip)
        {
            await connection.InvokeAsync("AddEquip", newEquip);
        }

        public async void AddHistory(History history)
        {
            await connection.InvokeAsync("AddHistory", history);
        }

        public async void AddType(Type type)
        {
            await connection.InvokeAsync("AddType", type);
        }

        public async void AddMOL(MOL mol)
        {
            await connection.InvokeAsync("AddMOL", mol);
        }

        public async void AddRoom(Room newRoom)
        {
            await connection.InvokeAsync("AddRoom", newRoom);
        }

        #endregion

        #region Remove

        public async void RemoveType(int selectedItemId)
        {
            await connection.InvokeAsync("RemoveType", selectedItemId);
        }

        public async void RemoveMOL(int selectedItemId)
        {
            await connection.InvokeAsync("RemoveMOL", selectedItemId);
        }

        public async void RemoveEquip(int id)
        {
            await connection.InvokeAsync("RemoveEquip", id);
        }

        public async void RemoveRoom(int roomId)
        {
            await connection.InvokeAsync("RemoveRoom", roomId);
        }

        #endregion

        #region Update

        public async void UpdateType(Type selectedItem)
        {
            await connection.InvokeAsync("UpdateType", selectedItem);
        }

        public async void UpdateMOL(MOL selectedItem)
        {
            await connection.InvokeAsync("UpdateMOL", selectedItem);
        }

        public async void UpdateRoom(Room room)
        {
            await connection.InvokeAsync("UpdateRoom", room);
        }

        public async void UpdateEquip(Equip equip)
        {
            await connection.InvokeAsync("UpdateEquip", equip);
        }

        #endregion

        public async void Decomission(int equipId)
        {
            await connection.InvokeAsync("Decomission", equipId);
        }
    }

}
