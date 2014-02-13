using System;
using Newtonsoft.Json;

namespace Perevorot.Web.Dtos
{    
    [JsonObject]
    public class CustomersGridResponseModel
    {
        [JsonProperty(Order = 1)]
        public long CustomerId;

        [JsonProperty(Order = 2)]
        public string CustomerName;

        [JsonProperty(Order = 3)]
        public DateTimeOffset CreationDate;

        [JsonProperty(Order = 4)]
        public long Calls;

        [JsonProperty(Order = 5)]
        public long Fields;

        [JsonProperty(Order = 6)]
        public string Details;
    }
}