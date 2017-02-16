using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class MetricEngineDTO
    {
        public MetricEngineDTO() { }

        public MetricEngineDTO(string name, string description, int? floor, int? ceiling, int? multiplier, int? xp)
        {
            this.Name = name;
            this.Description = description;
            this.Floor = floor;
            this.Ceiling = ceiling;
            this.Multiplier = multiplier;
            this.Xp = xp;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("floor")]
        public int? Floor { get; set; }

        [JsonProperty("ceiling")]
        public int? Ceiling { get; set; }

        [JsonProperty("multiplier")]
        public int? Multiplier { get; set; }

        [JsonProperty("xp")]
        public int? Xp { get; set; }

        [JsonProperty("media")]
        public bool IsAverage { get; set; }

        [JsonProperty("inverse")]
        public bool IsInverse { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}