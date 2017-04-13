using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class RunMetricEngineDTO
    {
        public RunMetricEngineDTO() { }

        public RunMetricEngineDTO(string runId, string playerId, string name, string description, int? floor, int? ceiling, int? multiplier, int? xp, int? score)
        {
            this.RunId = runId;
            this.PlayerId = playerId;
            this.Name = name;
            this.Description = description;
            this.Floor = floor;
            this.Ceiling = ceiling;
            this.Multiplier = multiplier;
            this.Xp = xp;
            this.Score = score;
        }

        [Key]
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("runId")]
        public string RunId { get; set; }

        [Required]
        [JsonProperty("playerId")]
        public string PlayerId { get; set; }

        [Required]
        [JsonProperty("metricId")]
        public string MetricId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

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

        [Required]
        [JsonProperty("points")]
        public float Points { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("arithmeticMultiplier")]
        public float ArithmeticMultiplier { get; set; } //Somente para a Fabiana

        //[JsonProperty("_links")]
        [JsonIgnore]
        public Link Links { get; set; }
    }
}

