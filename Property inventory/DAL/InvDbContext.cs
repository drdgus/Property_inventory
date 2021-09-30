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
                Statuses.Add(new Status { Name = "Ремонт" });
                Statuses.Add(new Status { Name = "Списано" });

                Categories.Add(new Category { Name = "Другое", Class = string.Empty });

                Categories.AddRange(new Category[]
                {
                    new Category { Name = "Компьютеры и периферийное оборудование", Class = "320.26.2"},
                    new Category { Name = "Оборудование для измерения, испытаний и навигации", Class = "330.26.51"},
                    new Category { Name = "Изделия текстильные готовые прочие", Class = "330.13.92.2"},
                    new Category { Name = "Бутылки стеклянные", Class = "330.23.13.11.110"},
                    new Category { Name = "Банки стеклянные", Class = "330.23.13.11.120"},
                    new Category { Name = "Тара прочая из стекла, кроме ампул", Class = "330.23.13.11.140"},
                    new Category { Name = "Посуда стеклянная для лабораторных, гигиенических или фармацевтических целей; ампулы из стекла", Class = "330.23.19.23"},
                    new Category { Name = "Изделия керамические лабораторного, химического или прочего технического назначения, кроме фарфоровых", Class = "330.23.44.12"},
                    new Category { Name = "Сейфы и контейнеры упрочненные металлические бронированные или армированные, специально предназначенные для хранения денег и документов", Class = "330.25.99.21.110"},
                    new Category { Name = "Часы всех видов", Class = "330.26.52"},
                    new Category { Name = "Приборы оптические и фотографическое оборудование", Class = "330.26.70"},
                    new Category { Name = "Носители информации магнитные и оптические", Class = "330.26.8"},
                    new Category { Name = "Мебель", Class = "330.31.01.12"},
                    new Category { Name = "Снаряды, инвентарь и оборудование для занятий физкультурой, гимнастикой и атлетикой, занятий в спортзалах, фитнес-центрах", Class = "330.32.30.14"},
                    new Category { Name = "Снаряды, инвентарь и оборудование прочие для занятий спортом или для игр на открытом воздухе; плавательные бассейны и бассейны для гребли", Class = "330.32.30.15"},
                    new Category { Name = "Мебель медицинская, включая хирургическую, стоматологическую или ветеринарную, и ее части", Class = "330.32.50.30.110"},
                    new Category { Name = "Метлы и щетки", Class = "330.32.91.1"},
                    new Category { Name = "Приборы, аппаратура и модели, предназначенные для демонстрационных целей", Class = "330.32.99.53"}
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
