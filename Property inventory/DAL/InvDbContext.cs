using Property_inventory.Entities;
using Property_inventory.Services;
using SQLite.CodeFirst;
using System;
using System.Data.Entity;
using System.Linq;
using History = Property_inventory.Entities.History;

namespace Property_inventory.DAL
{
    public class InvDbContext : DbContext
    {
        public DbSet<Equip> Equips { get; set; }
        public DbSet<Org> Orgs { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Entities.InvType> InvTypes { get; set; }
        public DbSet<Accountability> Accountabilities { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<MOL> MOLs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UnappliedChange> UnappliedChanges { get; set; }
        public DbSet<InvDocument> InvDocuments { get; set; }
        public DbSet<MOLPosition> MolPositions { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<CheckEquip> CheckEquips { get; set; }


        private static InvDbContext _instance;

        private InvDbContext() : base("DefaultConnection")
        {
            if (Orgs.Count() == 0)
            {
                Orgs.Add(new Org { Name = "МКОУ Таежнинская школа №20" });

                //Rooms.AddRange(Enumerable.Range(1, 20).Select(i => new Room { Name = $"Помещение {i}", OrgId = 1 }));

                Statuses.Add(new Status { Name = "На балансе" });
                Statuses.Add(new Status { Name = "Ремнот" });
                Statuses.Add(new Status { Name = "Списано" });

                Categories.AddRange(new Category[]
                {
                    new Category { Name = "Всё" }
                });

                Accountabilities.AddRange(new Accountability[]
                {
                    new Accountability { Name = "Основной баланс" },
                    new Accountability { Name = "З/б" },
                });

                MolPositions.Add(new MOLPosition { Name = "Зам. по АХЧ" });

                InvTypes.AddRange(new Entities.InvType[]
                {
                    new Entities.InvType{ Name = "МФУ", CategoryId = 1 },
                    new Entities.InvType{ Name = "Сканер", CategoryId = 1 },
                    new Entities.InvType{ Name = "Принтер", CategoryId = 1 },
                    new Entities.InvType{ Name = "Ноутбук", CategoryId = 1 },
                    new Entities.InvType{ Name = "Интерактивная доска", CategoryId = 1 },
                    new Entities.InvType{ Name = "ПК", CategoryId = 1 },
                    new Entities.InvType{ Name = "Монитор", CategoryId = 1 },
                    new Entities.InvType{ Name = "Стол учебный", CategoryId = 1 },
                    new Entities.InvType{ Name = "Стол офисный", CategoryId = 1 },
                    new Entities.InvType{ Name = "Стул учебный", CategoryId = 1 },
                    new Entities.InvType{ Name = "Стул офисный", CategoryId = 1 },
                });

                SaveChanges();

                //var rnd = new Random();
                //Equips.AddRange(Enumerable.Range(1, 1000)
                //    .Select(i => new Equip
                //    {
                //        Name = $"Имущество №{i}",
                //        RoomId = rnd.Next(1, 20),
                //        InvNum = i,
                //        Count = 1,
                //        MOLId = 1,
                //        TypeId = rnd.Next(1, 11),
                //        AccountabilityId = rnd.Next(1, 2),
                //        StatusId = rnd.Next(1, 3),
                //        RegistrationDate = DateTime.Now.AddDays(-rnd.Next(0, 100))
                //    })
                //);

                //SaveChanges();

                //var history = Equips.Select(i => new { Id = i.Id, RegistrationDate = i.RegistrationDate }).ToList().Select(i => new History
                //{
                //    ObjectId = i.Id,
                //    TableCode = InvEnums.Table.Equip,
                //    Date = i.RegistrationDate,
                //    Code = (InvEnums.OperationCode)rnd.Next(0, 5),
                //    ChangedProperty = (InvEnums.HistoryProperty)rnd.Next(0, 9),
                //    OldValue = "-",
                //    NewValue = "На балансе (создано автоматически)"
                //});

                //History.AddRange(history);

                //SaveChanges();
            }
        }

        public static InvDbContext GetInstance()
        {
            return _instance ?? (_instance = new InvDbContext());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<InvDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
