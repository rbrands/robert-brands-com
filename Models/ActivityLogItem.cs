using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace robert_brands_com.Models
{
    public class ActivityLogItem : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "user", NullValueHandling = NullValueHandling.Ignore)]
        public string User { get; set; }
        [JsonProperty(PropertyName = "messageTag", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageTag { get; set; }
        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "clientInfo", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientInfo { get; set; }
        [JsonProperty(PropertyName = "timeStamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TimeStamp { get; set; }
    }
}
