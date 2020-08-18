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


    public class devCommands : BaseCommandModule{
        [Command("isdev")]
        public async Task isDev(CommandContext ctx){
            if(await checkId(ctx.User.Id.ToString())){
                await ctx.Channel.SendMessageAsync("Yes, you are devloper").ConfigureAwait(false);
            }
            else{
                await ctx.Channel.SendMessageAsync("you no devloper").ConfigureAwait(false);
            }
        }
        [Command("createstarship")]
        public async Task createstarship(CommandContext ctx){
             if(await checkId(ctx.User.Id.ToString())){
                starship newShip;
                var interactivity = ctx.Client.GetInteractivity();
                await ctx.Channel.SendMessageAsync("What is the ship's formal ID? (all lower case)").ConfigureAwait(false);
                var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                
                string id = message.Result.Content;
                newShip.id = id;


                await ctx.Channel.SendMessageAsync("What is the ship's actual name? ").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                string propername = message.Result.Content;
                newShip.propername = propername;

                await ctx.Channel.SendMessageAsync("What is the ship's description? ").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                string description = message.Result.Content;
                newShip.description = description;

                await ctx.Channel.SendMessageAsync("What is the ship's speed? ").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
        
                int speed = Int32.Parse(message.Result.Content);
                newShip.speed = speed;

                await ctx.Channel.SendMessageAsync("What is the ship's base health? ").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                int health = Int32.Parse(message.Result.Content);
                newShip.health = health;

                await ctx.Channel.SendMessageAsync("What is the ship's cargo space? ").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
        
                int cargospace=  Int32.Parse(message.Result.Content);
                newShip.cargospace = cargospace;

                await ctx.Channel.SendMessageAsync("What is the ship's DPS? ").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
       
                float dps = float.Parse(message.Result.Content);
                newShip.dps = dps;

                string json = JsonConvert.SerializeObject(newShip);

                await ctx.Channel.SendMessageAsync("Ship successfully created!").ConfigureAwait(false);
                
                using (StreamWriter outputFile = new StreamWriter("starships/" + newShip.id + ".json"))
        {
            await outputFile.WriteAsync(json);
        }

                



            }
            else{
                await ctx.Channel.SendMessageAsync("you no devloper").ConfigureAwait(false);
            }
        }
        
        [Command("createsystem")]
        public async Task createSystem(CommandContext ctx){
            if(await checkId(ctx.User.Id.ToString())){
                await ctx.Channel.SendMessageAsync("test").ConfigureAwait(false);
                map existingMap;
                map newMap;
                string json;
                var interactivity = ctx.Client.GetInteractivity();
                using(var fs = File.OpenRead("systems/" + "map" + ".json"))
                using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);
            
                existingMap = JsonConvert.DeserializeObject<map>(json);
                newMap.sysIds = new string[existingMap.sysIds.Length + 1];

                for (int i = 0; i < existingMap.sysIds.Length; i++){
                
                    newMap.sysIds[i] = existingMap.sysIds[i];
                
                }

                await ctx.Channel.SendMessageAsync("What is the systems's formal ID? (all lower case)").ConfigureAwait(false);
                var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                
                string id = message.Result.Content;
                newMap.sysIds[newMap.sysIds.Length -1] = id;

                string outjson = JsonConvert.SerializeObject(newMap);

                
                
                using (StreamWriter outputFile = new StreamWriter("systems/map.json"))
        {
            await outputFile.WriteAsync(outjson);
        }   
                Directory.CreateDirectory("systems/" + id);

                SingleSystem newSys;
                newSys.sysId = id;

                await ctx.Channel.SendMessageAsync("What is the systems name?").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                newSys.name = message.Result.Content;

                await ctx.Channel.SendMessageAsync("What is the systems description?").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                newSys.description = message.Result.Content;

                await ctx.Channel.SendMessageAsync("What is the systems locations ID?").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                newSys.locations = message.Result.Content.Split(',');

                await ctx.Channel.SendMessageAsync("What is the systems neighbors ID?").ConfigureAwait(false);
                message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                newSys.neighbors = message.Result.Content.Split(',');

                string systemJson = JsonConvert.SerializeObject(newSys);

                
                
                using (StreamWriter outputFile = new StreamWriter("systems/" + id + "/" + id + ".json"))
        {
            await outputFile.WriteAsync(systemJson);
        }   



                await ctx.Channel.SendMessageAsync("System successfully created!").ConfigureAwait(false);
            }
            else{
                await ctx.Channel.SendMessageAsync("You no devlioper").ConfigureAwait(false);
            }

        }
        private async Task<bool> checkId(string id){
            await Task.Delay(1);
            devConfig devConfig;

            string json;
            using(var fs = File.OpenRead("devConfig.json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            devConfig = JsonConvert.DeserializeObject<devConfig>(json);

            return devConfig.id == id;
        }
    }




}
