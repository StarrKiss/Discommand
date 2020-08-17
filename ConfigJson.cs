using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace discBot
{
    public struct  ConfigJson
    {
        [JsonProperty("token")]
        public string Token {get; private set;}
        [JsonProperty("prefix")]
        public string Prefix {get; private set;}
    }
    
    
}
