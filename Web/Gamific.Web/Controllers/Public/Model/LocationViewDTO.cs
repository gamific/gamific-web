using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    public class LocationViewDTO
    {

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

    }
}