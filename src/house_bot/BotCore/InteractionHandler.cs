using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace root.src.house_bot.BotCore
{
    public class InteractionHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider provider)
    {
        public async Task InstallCommandsAsync()
        {
            await commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: provider);
            // client.Ready += async () => await commands.RegisterCommandsGloballyAsync();
            client.Ready += async () => await commands.RegisterCommandsToGuildAsync(1387915606870855957); //testing
            client.InteractionCreated += HandleInteraction;
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            await Task.Run(async () =>
            {
                var context = new SocketInteractionContext(client, interaction);
                var result = await commands.ExecuteCommandAsync(context, provider);
                await CatchExecutionErrors(context, result);
            });
        }

        private static async Task CatchExecutionErrors(IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.Error?.ToString() + " error.");
            }
        }
    }
}