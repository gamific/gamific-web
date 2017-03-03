using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class RunEngineDTO
    {
        public RunEngineDTO() { }

        public RunEngineDTO(long score, string playerId, string teamId)
        {
            this.Score = score;
            this.PlayerId = playerId;
            this.TeamId = teamId;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("playerId")]
        public string PlayerId { get; set; }

        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        [JsonProperty("logoId")]
        public int LogoId { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}