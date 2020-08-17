using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace discBot.commands{


    public class actualCommands : BaseCommandModule{
        
        [Command("uptime")]
        public async Task testMessage(CommandContext ctx){
            
             DateTime start = Process.GetCurrentProcess().StartTime;

             DateTime now = DateTime.Now;

            var difference = (now - start).TotalSeconds;

            await ctx.Channel.SendMessageAsync("Uptime is " + difference.ToString()).ConfigureAwait(false);

        }

         [Command("create")]
        public async Task createCharacter(CommandContext ctx){
            user newUser;
            string name;
            string gender;
            string id = ctx.User.Id.ToString();
            var interactivity = ctx.Client.GetInteractivity();
            await ctx.Channel.SendMessageAsync("What is your character's name? ").ConfigureAwait(false);
            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
            name = message.Result.Content;
            await ctx.Channel.SendMessageAsync("Got it, your character's name is " + name + ". What is your characters gender? This can be any string you want. ").ConfigureAwait(false);
            var genderMessage = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
            gender = genderMessage.Result.Content;
            await ctx.Channel.SendMessageAsync("Got it, your character's gender is " + gender + ". Creating your character now..... ").ConfigureAwait(false);
            newUser.gender = gender;
            newUser.id = id;
            newUser.charName = name;
            newUser.money = 0;
            newUser.ship = "Starhopper";
            newUser.system = "Earth";
            string json = JsonConvert.SerializeObject(newUser);

            await ctx.Channel.SendMessageAsync("Your character has been succesfully created!").ConfigureAwait(false);
            
            using (StreamWriter outputFile = new StreamWriter("users/" + newUser.id + ".json"))
        {
            await outputFile.WriteAsync(json);
        }




        }

        [Command("info")]
        public async Task info(CommandContext ctx){
            user existingUser;
            string id = ctx.User.Id.ToString();
            string json;

            using(var fs = File.OpenRead("users/" + id + ".json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            
            existingUser = JsonConvert.DeserializeObject<user>(json);
            var infoEmbed = new DiscordEmbedBuilder {
                Title = existingUser.charName,
                Description = "Gender: " + existingUser.gender + "\n Money: " + existingUser.money + "\n Starship: " + existingUser.ship + "\n System: " + existingUser.system
            };

            await ctx.Channel.SendMessageAsync(embed: infoEmbed).ConfigureAwait(false);
        }



    }


}

