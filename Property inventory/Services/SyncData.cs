using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Property_inventory.DAL;
using Property_inventory.Models;

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
                if (db.Orgs.Count() >= 1) return;
                var response = Client.GetAsync(ServerAddress+ "/All");
                response.Wait();

                var allTables = JsonConvert.DeserializeObject<AllTables>(response.Result.Content.ReadAsStringAsync().Result);

                db.Types.AddRange(allTables.Types);
                db.Rooms.AddRange(allTables.Rooms);
                db.MOLs.AddRange(allTables.MOLs);
                db.Equips.AddRange(allTables.Equips);
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
