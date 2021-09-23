using Property_inventory.Entities;
using SQLite.CodeFirst;
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
        public DbSet<Type> Types { get; set; }
        public DbSet<Accountability> Accountabilities { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<MOL> MOLs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UnappliedChange> UnappliedChanges { get; set; }
        public DbSet<InvDocument> InvDocuments { get; set; }
        public DbSet<MOLPosition> MolPositions { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }


        private static InvDbContext _instance;

        private InvDbContext() : base("DefaultConnection")
        {
            if (Orgs.Count() == 0)
            {
                Orgs.Add(new Org { Name = "МКОУ Таежнинская школа №20" });

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

                SaveChanges();
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
