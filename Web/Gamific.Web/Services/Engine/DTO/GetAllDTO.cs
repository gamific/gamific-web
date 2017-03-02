using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class GetAllDTO
    {
        [JsonProperty("_embedded")]
        public Embedded List { get; set; }

        [JsonProperty("page")]
        public Page PageInfo { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }

        public class Page
        {
            public int size { get; set; }
            public int totalElements { get; set; }
            public int totalPages { get; set;}
            public int number { get; set; }
        }

        public class Embedded
        {
            public List<MetricEngineDTO> metric { get; set; }
            public List<TeamEngineDTO> team { get; set; }
            public List<PlayerEngineDTO> player { get; set; }
            public List<RunEngineDTO> run { get; set; }
            public List<RunMetricEngineDTO> runMetric { get; set; }
            public List<EpisodeEngineDTO> episode { get; set; }
            public List<ResultEngineDTO> result { get; set; }
            public List<GameEngineDTO> game { get; set; }
            public List<ItemEngineDTO> item { get; set; }
        }
    }
}