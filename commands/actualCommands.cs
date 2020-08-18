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
            newUser.ship = "starhopper";
            newUser.system = "earth";
            newUser.shiphealth = 100;
            newUser.cargo = new string[5];
            newUser.cargo[0] = "An old BlockBuster video card";
            newUser.cargo[1] = "A letter from your dead mother mocking your failure to get a job.";
            newUser.systemarea = "station";
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
            discBot.tools.shipTools tools = new discBot.tools.shipTools();
            string id = ctx.User.Id.ToString();
            
            existingUser = await tools.readUser(id);
            
            starship existingShip;
            
            
            existingShip = await tools.readShip(existingUser.ship);

            var infoEmbed = new DiscordEmbedBuilder {
                Title = existingUser.charName,
                Description = "Gender: " + existingUser.gender + "\n Money: " + existingUser.money + "\n Starship: " + existingShip.propername + "\n System: " + existingUser.system
            };

            await ctx.Channel.SendMessageAsync(embed: infoEmbed).ConfigureAwait(false);
        }

        [Command("changeship")]
        public async Task changeship(CommandContext ctx, string shipid){
            discBot.tools.shipTools tools = new discBot.tools.shipTools();
            var interactivity = ctx.Client.GetInteractivity();
            await ctx.Channel.SendMessageAsync("If the ship you are changing to has less cargo space than your current ship, some cargo will be lost. hit y to continue, or any other key to exit.");
            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
            if(message.Result.Content == "y"){
                await tools.changeShip(shipid, ctx.User.Id.ToString());
                await ctx.Channel.SendMessageAsync("Ship succesfully changed");
            }
            
        }

        [Command("dashboard")]
        public async Task dashboard(CommandContext ctx){
            user existingUser;
            string id = ctx.User.Id.ToString();
            string json;

            using(var fs = File.OpenRead("users/" + id + ".json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            
            existingUser = JsonConvert.DeserializeObject<user>(json);
            
            starship existingShip;
            string shipJson;
            using(var fs = File.OpenRead("starships/" + existingUser.ship + ".json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                shipJson = await sr.ReadToEndAsync().ConfigureAwait(false);
            
            existingShip = JsonConvert.DeserializeObject<starship>(shipJson);

            int length = existingShip.cargospace;

            string cargo = "";

            for (int i = 0; i < length; i++){
                if(existingUser.cargo[i] != null){
                    cargo = cargo + existingUser.cargo[i];
                }
                else{
                    cargo = cargo + "Free";
                }

                cargo += ", ";
            }
            
            var infoEmbed = new DiscordEmbedBuilder {
                Title = "Welcome to " + existingUser.systemarea + " in " + existingUser.system + ", " + existingUser.charName,
                Description = "Your health: " + existingUser.shiphealth + "\n Your money: " + existingUser.money + "\n Your cargo: " + cargo + " \n You have " + (existingUser.cargo.Length - existingUser.cargo.Count(i => i != null)).ToString() + " free spaces"
            };

            await ctx.Channel.SendMessageAsync(embed: infoEmbed).ConfigureAwait(false);

        }

        [Command("shipinfo")]
        public async Task shipinfo(CommandContext ctx){
            discBot.tools.shipTools tools = new discBot.tools.shipTools();
            user existingUser;
            starship existingShip;
            string id = ctx.User.Id.ToString();
            

            
            
            existingUser = await tools.readUser(id);
            
            
            existingShip = await tools.readShip(existingUser.ship);
            
            var infoEmbed = new DiscordEmbedBuilder {
                Title = existingShip.propername,
                Description =  existingShip.description + "\n Health: " + existingUser.shiphealth + "/" + existingShip.health + "\n Speed: " + existingShip.speed + "\n DPS: " + existingShip.dps + "\n Cargo: " + existingShip.cargospace,
                
            };

            

            await ctx.Channel.SendMessageAsync(embed: infoEmbed).ConfigureAwait(false);

            
        }

        [Command("sysinfo")]
        public async Task sysinfo(CommandContext ctx){
            discBot.tools.shipTools tools = new discBot.tools.shipTools();
            SingleSystem existingSystem;
            user existingUser;

            string id = ctx.User.Id.ToString();
            
            
            existingUser = await tools.readUser(id);

            string sysJson;
            using(var fs = File.OpenRead("systems/" + existingUser.system + "/" + existingUser.system + ".json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                sysJson = await sr.ReadToEndAsync().ConfigureAwait(false);
            
            existingSystem = JsonConvert.DeserializeObject<SingleSystem>(sysJson);

            

            string neighbors = string.Join(",", existingSystem.neighbors);
            string Locations = string.Join(",", existingSystem.locations);


            var infoEmbed = new DiscordEmbedBuilder {
                Title = existingSystem.name,
                Description =  "Description: " + existingSystem.description + "\n \n Neighbors: " + neighbors + "\n Locations: " + Locations 
                
            };

            await ctx.Channel.SendMessageAsync(embed: infoEmbed).ConfigureAwait(false);

        }

        [Command("jump")]
        public async Task warp(CommandContext ctx){
            discBot.tools.shipTools tools = new discBot.tools.shipTools();
            await ctx.Channel.SendMessageAsync("Want to go to another system, or to another location here? \n (s for in this system, l for another system)").ConfigureAwait(false);
            var interactivity = ctx.Client.GetInteractivity();
            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
            
            SingleSystem existingSystem;
            user existingUser;

            string id = ctx.User.Id.ToString();
            
            user newUser;

            
            
            existingUser = await tools.readUser(id);
            newUser = existingUser;

            string sysJson;
            using(var fs = File.OpenRead("systems/" + existingUser.system + "/" + existingUser.system + ".json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                sysJson = await sr.ReadToEndAsync().ConfigureAwait(false);
            
            existingSystem = JsonConvert.DeserializeObject<SingleSystem>(sysJson);

            if(message.Result.Content == "l"){
                string output = "";
                for(int i = 0; i < existingSystem.neighbors.Length; i++){
                    string tempJson;
                    SingleSystem tempSystem;

                    
                    using(var fs = File.OpenRead("systems/" + existingSystem.neighbors[i] + "/" + existingSystem.neighbors[i] + ".json"))
                    using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                tempJson = await sr.ReadToEndAsync().ConfigureAwait(false);
            
                 tempSystem = JsonConvert.DeserializeObject<SingleSystem>(tempJson);


                    output = output + i + ". " + tempSystem.name + "\n";
                }   
                await ctx.Channel.SendMessageAsync(output).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync("Choose your planet!").ConfigureAwait(false);
                
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                newUser.system = existingSystem.neighbors[Int32.Parse(message.Result.Content)];

                SingleSystem destination;
                string outJson;
                using(var fs = File.OpenRead("systems/" + existingSystem.neighbors[Int32.Parse(message.Result.Content)] + "/" + existingSystem.neighbors[Int32.Parse(message.Result.Content)] + ".json"))
                using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                outJson = await sr.ReadToEndAsync().ConfigureAwait(false);
            
                 destination = JsonConvert.DeserializeObject<SingleSystem>(outJson);

                 newUser.systemarea = destination.locations[0];

                 string finaljson = JsonConvert.SerializeObject(newUser);
            
            await ctx.Channel.SendMessageAsync("Welcome to " + destination.name + "!").ConfigureAwait(false);
            
            using (StreamWriter outputFile = new StreamWriter("users/" + newUser.id + ".json"))
        {
            await outputFile.WriteAsync(finaljson);
        }
            }

            
            
        }
    }


}

