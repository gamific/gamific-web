using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    public class LocationDTO
    {

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("zoom")]
        public int Zoom { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("playerId")]
        public string PlayerId { get; set; }


    }
}