using Discord;
using Discord.Interactions;
using root.src.house_bot.DB;

namespace root.src.house_bot.Commands
{
    public class TestGetLatestRoomDef : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("test22", "...")]
        [CommandContextType(InteractionContextType.Guild)]
        public async Task GetLatestRoomDef()
        {
            string? latest = await RoomsDBMethods.GetGuildDefinition(Context.Guild.Id);
            await RespondAsync("Sent in DM!", ephemeral: true);
            await SendDM(latest ?? "null");
        }
        public async Task SendDM(string input)
        {
            var user = Context.User;
            var chunked = input.Chunk(2000);
            foreach (var chunk in chunked)
            {
                await user.SendMessageAsync(string.Join("", chunk));
            }
        }
    }
}