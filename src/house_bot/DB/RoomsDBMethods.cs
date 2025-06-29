using Microsoft.EntityFrameworkCore;

namespace root.src.house_bot.DB
{
    public static class RoomsDBMethods
    {
        public static async Task SetGuildDefinition(string json, ulong guildId)
        {
            await using var db = new DatabaseContext();
            JsonDefinition? existing = await db.JsonDefsTable.FirstOrDefaultAsync(d => d.GuildID == guildId);

            if (existing != null)
                existing.DefinitionJson = json;
            else
            {
                db.Add(new JsonDefinition
                {
                    GuildID = guildId,
                    DefinitionJson = json,
                }
                );
            }
            await db.SaveChangesAsync();
        }

        public static async Task<string?> GetGuildDefinition(ulong guildId)
        {
            await using var db = new DatabaseContext();
            JsonDefinition? lastest = await db.JsonDefsTable
            .FirstOrDefaultAsync(d => d.GuildID == guildId);
            return lastest?.DefinitionJson;
        }
    }
}