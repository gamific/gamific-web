using Newtonsoft.Json;
using System.Collections.Generic;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    /// <summary>
    /// Classe com informações para buscar resultados dos graficos
    /// </summary>
    public class ChartResultDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("entries")]
        public List<EpisodeEngineDTO> Entries { get; set; }
    }
}