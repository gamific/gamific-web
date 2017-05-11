using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class ChartParamDTO
    {

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("metrics")]
        public List<MetricEngineDTO> Metrics { get; set; }

        [JsonProperty("initDate")]
        public long InitDate { get; set; }

        [JsonProperty("finishDate")]
        public long FinishDate { get; set; }

    }
}