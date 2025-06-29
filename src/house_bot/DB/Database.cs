using Microsoft.EntityFrameworkCore;

namespace root.src.house_bot.DB
{
    public static class Database
    {
        public static async Task InitializeAsync()
        {
            await using var db = new DatabaseContext();
            await db.Database.EnsureCreatedAsync();
        }
    }
    public class DatabaseContext : DbContext
    {
        public DbSet<JsonDefinition> JsonDefsTable { get; set; }
        public DbSet<AddedChannels> AddedChannelsTable { get; set; }

        public string DbPath { get; }

        public DatabaseContext()
        {
            var folder = Environment.SpecialFolder.ApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "Username0103", "HouseBot", "HouseBot.db");
            string dbDir = Path.GetDirectoryName(DbPath)!;
            Directory.CreateDirectory(dbDir);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}