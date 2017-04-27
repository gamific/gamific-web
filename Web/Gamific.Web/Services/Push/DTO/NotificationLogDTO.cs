using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Push.DTO
{
    public class NotificationLogDTO
    {
        [JsonProperty("multicast_id")]
        public float MulticastId { get; set; }

        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("canonical_ids")]
        public string CanonicalIds { get; set; }

        [JsonProperty("results")]
        public Result Results { get; set; }

        public class Result
        {
            [JsonProperty("message_id")]
            List<string> message_id { get; set; }
        }
    }
}