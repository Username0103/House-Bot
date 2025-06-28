using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using root.src.house_bot.BotCore;

namespace root.src.house_bot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var services = CreateServices();
            string token = Misc.GetToken.Get();
            var client = services.GetRequiredService<DiscordSocketClient>();
            var commandHandler = services.GetRequiredService<InteractionHandler>();
            client.Log += Misc.Logging.Log;
            await commandHandler.InstallCommandsAsync();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private static ServiceProvider CreateServices()
        {
            var commandConfig = new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                ThrowOnError = true
            };
            var socketConfig = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            var collection = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(socketConfig))
                .AddSingleton(new CommandService(commandConfig))
                .AddSingleton<DiscordRestClient>()
                .AddSingleton(new InteractionServiceConfig { })
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>();

            return collection.BuildServiceProvider();
        }
    }
}