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

namespace discBot.tools{

    public class shipTools{
    public async Task changeShip(string shipId, string id){
        user existingUser;
        await Task.Delay(1);
        string json;

        using(var fs = File.OpenRead("users/" + id + ".json"))
        using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync().ConfigureAwait(false);
            
        existingUser = JsonConvert.DeserializeObject<user>(json);
        
        starship newShip;
        string shipJson;
        using(var fs = File.OpenRead("starships/" + shipId + ".json"))
        using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                shipJson = await sr.ReadToEndAsync().ConfigureAwait(false);
            
        newShip = JsonConvert.DeserializeObject<starship>(shipJson);
        
        existingUser.ship = newShip.id;

        existingUser.shiphealth = newShip.health;

        string[] tempArray = existingUser.cargo;

        string[] newArray = new string[newShip.cargospace];

        

        for (int i = 0; i < tempArray.Count(i => i != null); i++){
                if(i < newArray.Length){
                    newArray[i] = tempArray[i];
                }
        }
        

        existingUser.cargo = newArray;
          string newson = JsonConvert.SerializeObject(existingUser);
            
        
            
        using (StreamWriter outputFile = new StreamWriter("users/" + id + ".json"))
        {
            await outputFile.WriteAsync(newson);
        }
        
            
        
    } 

    }


}