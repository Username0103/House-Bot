
using Discord.Interactions;

namespace root.src.house_bot.BotCore
{
    public class TestCommands : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Do it!!")]
        public async Task PingAsync()
        {
            await RespondAsync($"Pong! {Context.Client.Latency}ms");
        }
    }
}