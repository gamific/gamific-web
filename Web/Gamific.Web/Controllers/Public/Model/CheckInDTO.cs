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
        public string Date { get; set; }

        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }
}
