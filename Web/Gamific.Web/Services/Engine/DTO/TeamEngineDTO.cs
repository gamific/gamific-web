using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class TeamEngineDTO
    {
        public TeamEngineDTO() { }

        public TeamEngineDTO(string episodeId, string nick)
        {
            this.EpisodeId = episodeId;
            this.Nick = nick;
        }

        [Key]
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Episodio é obrigatorio.")]
        [JsonProperty("episodeId")]
        public string EpisodeId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatorio.")]
        [JsonProperty("nick")]
        public string Nick { get; set; }

        [Required(ErrorMessage = "Responsavel é obrigatorio.")]
        [JsonProperty("masterPlayerId")]
        public string MasterPlayerId { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("logoId")]
        public int LogoId { get; set; }

        [JsonProperty("logoPath")]
        public string LogoPath { get; set; }
    }
}