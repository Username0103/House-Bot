
namespace root.src.house_bot.DB
{
    public class JsonDefinition
    {
        public int Id { get; set; }
        public required ulong GuildID { get; set; }
        public required string DefinitionJson { get; set; }
    }
    public class AddedChannels
    {
        public int Id { get; set; }
        public required ulong DiscordID { get; set; }
        public required ulong GuildID { get; set; }
        public required bool IsCategory { get; set; }
    }
}