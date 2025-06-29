
using Microsoft.EntityFrameworkCore;

namespace root.src.house_bot.DB
{
    public static class AddedChannelsDbMethods
    {
        public static async Task AddChannel(ulong id, ulong guildId, bool IsCategory)
        {
            await using var db = new DatabaseContext();
            db.Add(new AddedChannels
            {
                DiscordID = id,
                GuildID = guildId,
                IsCategory = IsCategory
            }
            );
            await db.SaveChangesAsync();
        }
        public static async Task<AddedChannels[]> GetChannels(ulong guildId)
        {
            await using var db = new DatabaseContext();
            return await db.AddedChannelsTable.Where((c) => c.GuildID == guildId).ToArrayAsync();
        }
        public static async Task RemoveAllFromGuild(ulong guildId)
        {
            await using var db = new DatabaseContext();
            await db.AddedChannelsTable
            .Where((t) =>
            t.GuildID == guildId)
            .ExecuteDeleteAsync();
        }
    }
}