using Discord;
using Discord.Interactions;
using Discord.Rest;
using Newtonsoft.Json.Linq;
using root.src.house_bot.DB;

namespace root.src.house_bot.Commands
{
    public class Refresh : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("refresh", "Create the channels and categories " +
        " as defined by the JSON room definition.")]
        [CommandContextType(InteractionContextType.Guild)]
        [RequireUserPermission(ChannelPermission.ManageChannels)]
        public async Task Create()
        {
            await DeferAsync(ephemeral: true);
            await DeleteOld();

            string? roomsJson = await RoomsDBMethods
                .GetGuildDefinition(Context.Guild.Id);
            if (roomsJson == null)
            {
                await FollowupAsync("You need to run /add_room_definition" +
            " and add your definition before this.", ephemeral: true); return;
            }
            var mapConfig = JObject.Parse(roomsJson);

            if (mapConfig == null)
            {
                await FollowupAsync("JSON parsing error, please re-try adding" +
            " the rooms configuration.", ephemeral: true); return;
            }

            await ParseJSON(mapConfig);
            await FollowupAsync("Finished setting up channels.", ephemeral: true);
        }
        private async Task DeleteOld()
        {
            AddedChannels[] old = await AddedChannelsDbMethods
                .GetChannels(Context.Guild.Id);

            foreach (var channel in old)
            {
                var channelSocket = Context.Guild.GetChannel(channel.DiscordID);
                if (channelSocket != null) { await channelSocket.DeleteAsync(); }
            }
            await AddedChannelsDbMethods.RemoveAllFromGuild(Context.Guild.Id);
        }
        private async Task ParseJSON(JObject mapConfig)
        {
            var meta = mapConfig["meta"]?.DeepClone();
            if (meta == null) { return; }
            mapConfig.Remove("meta");
            foreach (var category in mapConfig)
            {
                var createdCategory = await Context.Guild
                    .CreateCategoryChannelAsync(category.Key);
                await AddedChannelsDbMethods.AddChannel(
                    createdCategory.Id, Context.Guild.Id, true
                );

                await AddAllowedRoles(createdCategory,
                    meta?["AccessibleByRolesIds"]?
                        .ToObject<List<string>>(), true);

                if (category.Value != null)
                    foreach (var room in category.Value)
                    {
                        Action<GuildChannelProperties> channelProperties = new((props) =>
                        {
                            props.CategoryId = createdCategory.Id;
                        });

                        var createdChannel = await Context.Guild
                            .CreateTextChannelAsync(((JProperty)room).Name, channelProperties);
                        await AddedChannelsDbMethods
                            .AddChannel(createdChannel.Id, Context.Guild.Id, false);

                        Overwrite[] overwrite =
                        [
                        new Overwrite(
                            Context.Guild.EveryoneRole.Id,
                            PermissionTarget.Role,
                            new OverwritePermissions(viewChannel:
                            ((JProperty?)meta?["StartRoom"])?
                            .Value.ToObject<string>() == ((JProperty)room).Name ?
                            PermValue.Allow :
                            PermValue.Deny)
                            )
                        ];

                        await createdChannel.ModifyAsync(props =>
                            props.PermissionOverwrites = overwrite);
                    }
            }
        }
        private async Task AddAllowedRoles(RestCategoryChannel category,
            List<string>? roles, bool defaultAllow)
        {
            if ((defaultAllow && roles == null) || roles == null) { return; }
            if (!ParseRoles(roles, out var roleNums) || roleNums == null) { return; }
            List<Overwrite> overwrites = [new Overwrite(
                    Context.Guild.EveryoneRole.Id,
                    PermissionTarget.Role,
                    new OverwritePermissions(viewChannel: PermValue.Deny))];

            foreach (var role in roleNums)
            {
                overwrites.Add(new Overwrite(role, PermissionTarget.Role,
                    new OverwritePermissions(viewChannel: PermValue.Allow)));
            }
            await category.ModifyAsync(props =>
                props.PermissionOverwrites = overwrites);
        }
        private static bool ParseRoles(List<string>? roles, out List<ulong>? result)
        {
            if (roles == null) { result = null; return true; }
            List<ulong> ulongs = [];
            foreach (string role in roles)
            {
                if (!ulong.TryParse(role, out var number))
                { result = null; return false; }
                ulongs.Add(number);
            }
            result = ulongs;
            return true;
        }
    }
}