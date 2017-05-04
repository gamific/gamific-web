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
        public float Multicast_Id { get; set; }

        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("canonical_ids")]
        public string Canonical_Ids { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        public class Result
        {
            [JsonProperty("message_id")]
            public string Message_id { get; set; }

            [JsonProperty("registration_id")]
            public string registration_id { get; set; }

            [JsonProperty("error")]
            public string error { get; set; }
        }
    }
}