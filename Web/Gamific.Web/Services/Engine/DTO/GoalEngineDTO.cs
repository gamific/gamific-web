using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class GoalEngineDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("runId")]
        public string RunId { get; set; }

        [JsonProperty("goal")]
        public float Goal { get; set; }

        [JsonProperty("percentage")]
        public long Percentage { get; set; }

        [JsonProperty("metricId")]
        public string MetricId { get; set; }

        [JsonProperty("metricName")]
        public string MetricName { get; set; }

        [JsonProperty("metricIcon")]
        public string MetricIcon { get; set; }
    }
}