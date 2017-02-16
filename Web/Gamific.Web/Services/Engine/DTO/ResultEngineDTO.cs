using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class ResultEngineDTO
    {
        [JsonProperty("totalPoints")]
        public int TotalPoints { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }

        [JsonProperty("logoId")]
        public int LogoId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}