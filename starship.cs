using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace discBot
{
    public struct  starship
    {
        [JsonProperty("id")]
        public string id;
        [JsonProperty("name")]
        public string propername;

        [JsonProperty("description")]
        public string description;

        [JsonProperty("speed")]
        public int speed;
        [JsonProperty("health")]
        public int health;
        [JsonProperty("cargospace")]
        public int cargospace;
         [JsonProperty("dps")]
        public float dps;


        
    }
    
    
}
