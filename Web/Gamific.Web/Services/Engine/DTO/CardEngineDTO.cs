using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class CardEngineDTO
    {
        [JsonProperty("totalPoints")]
        public int TotalPoints { get; set; }

        [JsonProperty("metricName")]
        public string MetricName { get; set; }

        [JsonProperty("iconMetric")]
        public string IconMetric { get; set; }

        [JsonProperty("media")]
        public bool IsAverage { get; set; }

        [JsonProperty("inverse")]
        public bool IsInverse { get; set; }

        [JsonProperty("metricId")]
        public string MetricId { get; set; }

        [JsonProperty("percentGoal")]
        public float PercentGoal { get; set; }

        [JsonProperty("goal")]
        public int Goal { get; set; }
    }
}