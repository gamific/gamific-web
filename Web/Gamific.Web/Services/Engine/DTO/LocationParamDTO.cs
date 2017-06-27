using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class LocationParamDTO
    {

        [JsonProperty("metric")]
        public MetricEngineDTO Metrics { get; set; }

        [JsonProperty("runners")]
        public List<RunEngineDTO> Runners { get; set; }

    }
}