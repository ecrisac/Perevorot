using System;
using Newtonsoft.Json;

namespace Perevorot.Web.Dtos
{    
    [JsonObject]
    public class CustomerDto
    {
        [JsonProperty(Order = 1)]
        public long Id;

        [JsonProperty(Order = 2)]
        public string CustomerName;

        [JsonProperty(Order = 3)]
        public DateTimeOffset CreationDate;
    }
}