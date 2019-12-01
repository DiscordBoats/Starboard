using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.IO;
using Discord;
using System;

namespace Starboat.Services
{
    // Credit: https://github.com/Aux/Discord.Net-Example/blob/2.0/src/Services/LoggingService.cs
    public class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private string logDirectory { get; }
        private string logFile => Path.Combine(logDirectory, $"${DateTime.UtcNow.ToString("mm-dd-yyyy")}.log");

        public LoggingService(DiscordSocketClient client)
        {
            logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
            _discord  = client;

            _discord.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
            if (!File.Exists(logFile)) File.Create(logFile).Dispose();

            var text = $"[{DateTime.UtcNow.ToString("hh:mm:ss tt")} [{msg.Severity}] {msg.Source}  ->  {msg.Exception?.ToString() ?? msg.Message}";
            File.AppendAllText(logFile, $"{text}\n");
            return Console.Out.WriteLineAsync(text);
        }
    }
}