using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.IO;
using Newtonsoft.Json;

namespace discBot
{
    class Bot
    { 
        public DiscordClient Client {get; private set;}

        public InteractivityExtension interactivity{get; private set;}
        public CommandsNextExtension Commands {get; private set;}
        public async Task runAsync(){
            
            string json = string.Empty;

            using(var fs = File.OpenRead("config.json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            DiscordConfiguration config = new DiscordConfiguration{
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

             Client = new DiscordClient(config);

             Client.Ready += OnClientReady;

             Client.UseInteractivity(new InteractivityConfiguration{
                 Timeout = TimeSpan.FromMinutes(5)
             });

             var commandsConfig = new CommandsNextConfiguration{
                 StringPrefixes = new string[]{configJson.Prefix},
                 EnableDms = false,
                 EnableMentionPrefix = true,
                 DmHelp = false

             };
             
             Commands = Client.UseCommandsNext(commandsConfig);

             Commands.RegisterCommands<discBot.commands.actualCommands>();
            
            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e){
            
            return Task.CompletedTask;
        }
    }
}
