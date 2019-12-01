using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Text;
using System.Linq;
using Discord;
using System;

namespace Starboat.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient discord;
        private readonly IConfigurationRoot config;
        private readonly IServiceProvider provider;
        private readonly CommandService commands;

        public CommandHandler(DiscordSocketClient _discord, IConfigurationRoot root, IServiceProvider _provider, CommandService service)
        {
            discord = _discord;
            config = root;
            provider = _provider;
            commands = service;

            discord.MessageReceived += OnMessageAsync;
        }

        // credit: https://github.com/Aux/Dogey/blob/master/src/Dogey/Services/Background/CommandHandlingService.cs
        private async Task OnMessageAsync(SocketMessage message)
        {
            if (!(message is SocketUserMessage)) return;

            var context = new SocketCommandContext(discord, message as SocketUserMessage);
            if (IsCommand(message as SocketUserMessage, out int argPos))
            {
                var args = context.Message.Content.Substring(argPos);
                await ExecuteCommandAsync(context, args);
            }
        }

        private bool IsCommand(SocketUserMessage msg, out int argPos)
        {
            argPos = 0;

            if (msg.Author.IsBot) return false;
            if (msg.Author.Id == discord.CurrentUser.Id) return false;
            var hasPrefix = msg.HasStringPrefix(config["discord:prefix"], ref argPos);
            return hasPrefix || msg.HasMentionPrefix(discord.CurrentUser, ref argPos);
        }

        private async Task ExecuteCommandAsync(SocketCommandContext context, string input)
        {
            var result = await commands.ExecuteAsync(context, input, provider);
            if (result.IsSuccess || result.Error == CommandError.UnknownCommand) return;
            if (result is ParseResult parse && parse.Error == CommandError.BadArgCount)
            {
                var command = commands.Search(context, input)
                    .Commands
                    .OrderByDescending(x => x.Command.Parameters.Count)
                    .FirstOrDefault()
                    .Command;

                var prefix = config["discord:prefix"];
                var str = new StringBuilder($"{prefix}{command.Name}");
                if (command.Parameters.Count > 0)
                {
                    foreach (var arg in command.Parameters)
                    {
                        string argu = arg.Name;
                        if (arg.IsRemainder) argu += "...";
                        if (arg.IsMultiple) argu += "|";
                        if (arg.IsOptional)
                        {
                            argu = $"[{argu}";
                            if (arg.DefaultValue != null) argu += $"={arg.DefaultValue}";
                            argu += "]";
                        }
                        else
                        {
                            argu += $"<{argu}>";
                        }

                        str.Append($" {argu}");
                    }
                }

                var embed = new EmbedBuilder()
                    .WithTitle("Starboat | Invalid Command Usage")
                    .WithDescription($"**Usage**: `{str}`")
                    .Build();

                await context.Channel.SendMessageAsync(embed: embed);
                return;
            }

            if (!string.IsNullOrWhiteSpace(result.ErrorReason)) await context.Channel.SendMessageAsync($"Unable to run: `{result.ErrorReason}`");
        }
    }
}