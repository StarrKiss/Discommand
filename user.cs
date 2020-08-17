using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace discBot
{
    public struct  user
    {
        [JsonProperty("id")]
        public string id;
        [JsonProperty("name")]
        public string charName;
        [JsonProperty("gender")]
        public string gender;
        [JsonProperty("money")]
        public int money;
        [JsonProperty("ship")]
        public string ship;
         [JsonProperty("system")]
        public string system;


        
    }
    
    
}
