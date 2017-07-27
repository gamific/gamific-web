using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    /// <summary>
    /// Summary description for CheckInDTO
    /// </summary>
    public class CheckInDTO
    {

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("playerId")]
        public string PlayerId { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

    }
}
