using Discord;

namespace root.src.house_bot.Misc
{
    public static class Logging
    {
        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}