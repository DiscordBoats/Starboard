using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace Starboat.Services
{
    public class StarboardService
    {
        private readonly string _star = "⭐";
        private DiscordSocketClient _client;
        private readonly IConfigurationRoot _config;

        public StarboardService(DiscordSocketClient client, IConfigurationRoot root)
        {
            _client = client;
            _config = root;
            
            _client.ReactionAdded += OnReactionAddedAsync;
            _client.MessageDeleted += OnMessageDeletedAsync;
            _client.ReactionRemoved += OnReactionRemoveAsync;
            _client.ReactionsCleared += OnAllReactionsClearedAsync;
        }

        // all code here was used by Builderb's old Starboat source code (pls give him love <3)
        private async Task OnReactionAddedAsync(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (channel is SocketDMChannel || !reaction.User.IsSpecified || reaction.User.Value.IsBot || reaction.Emote.Name != _star) return;

            // get the values before doing anything
            var msg = await message.GetOrDownloadAsync();
            var chanID = _config["starboard:channelID"];
            
            // do some epic jeff
        }
    }
}