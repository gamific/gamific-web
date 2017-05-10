using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class BarParamDTO
    {

        [JsonProperty("episodes")]
        public List<EpisodeEngineDTO> Episodes { get; set; }

        [JsonProperty("metrics")]
        public List<MetricEngineDTO> Metrics { get; set; }

    }
}