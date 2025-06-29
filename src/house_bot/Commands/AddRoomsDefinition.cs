using Discord;
using Discord.Interactions;
using root.src.house_bot.DB;
using root.src.house_bot.Misc;

namespace root.src.house_bot.Commands
{
    public class AddRoomsDefinition(SchemaValidate schema, HttpClient client) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("add_room_definition", "Upload a JSON file to define room layouts. See /help_admin for more info")]
        [CommandContextType(InteractionContextType.Guild)]
        [RequireUserPermission(ChannelPermission.ManageChannels)]
        public async Task AddDefinition(Attachment attachment)
        {
            await DeferAsync(ephemeral: true);
            var (success, content) = await ReadAttachment(attachment);
            if (!success)
            {
                await FollowupAsync(content, ephemeral: true);
                return;
            }
            string? errors = await schema.ValidateJson(content);
            if (errors == null)
            {
                await RoomsDBMethods.SetGuildDefinition(content, Context.Guild.Id);
                await FollowupAsync("Successfully added room definition. Run /refresh to update server.", ephemeral: true);
            }
            else
                await FollowupAsync(errors, ephemeral: true);

        }

        private async Task<(bool Success, string Content)> ReadAttachment(Attachment attachment)
        {
            if (!attachment.Filename.ToLower().EndsWith(".json"))
            {
                return (false, "File is not a .JSON file.");
            }
            if (attachment.Size > 100_000)
            {
                return (false, $"JSON size ({attachment.Size / 1024}KB) exceeds 100KB limit.");
            }
            HttpResponseMessage responseHttp = await client.GetAsync(attachment.Url);
            return (true, await responseHttp.Content.ReadAsStringAsync());
        }
    }
}