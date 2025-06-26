using dotenv.net;

namespace root.src.house_bot.misc
{
    public static class GetToken
    {
        public static string Get()
        {
            IDictionary<string, string> denv =
                DotEnv.Fluent().
                WithTrimValues().
                WithoutExceptions().
                Read();
            denv.TryGetValue("discordBotToken", out string? token);
            if (token == null)
            {
                Console.WriteLine("Please create a file named \".env\" in your current directory," +
                " with discordBotToken=\"<Your discord bot token here>\" being the content.");
                Environment.Exit(1);
                return "";
            }
            else { return token; }
        }
    }
}

