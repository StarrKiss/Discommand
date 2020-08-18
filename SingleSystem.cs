using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace discBot
{
    public struct  SingleSystem
    {
        [JsonProperty("id")]
        public string sysId;

        [JsonProperty("name")]
        public string name;

        [JsonProperty("description")]

        public string  description;

        [JsonProperty("locations")]

        public string[] locations;
        
        //ignore this

        [JsonProperty("neighbors")]
        public string[] neighbors;

        
    }
    
    
}
