using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vlast.Gamific.Model.Firm.DTO;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class HallOfFameEngineDTO
    {
        public HallOfFameEngineDTO() { }

        public HallOfFameEngineDTO(string id, string episodeName, string episodeId, string gameId, List<RunEngineDTO> generalWinners, List<EngineTeamDTO> teamWinners)
        {
            this.Id = id;
            this.EpisodeName = episodeName;
            this.EpisodeId = episodeId;
            this.GameId = gameId;
            this.GeneralWinners = generalWinners;
            this.TeamWinners = teamWinners;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("episodeName")]
        public string EpisodeName { get; set; }

        [JsonProperty("episodeId")]
        public string EpisodeId { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("generalWinners")]
        public List<RunEngineDTO> GeneralWinners { get; set; }

        [JsonProperty("ceiling")]
        public int? Ceiling { get; set; }

        [JsonProperty("teamWinners")]
        public List<EngineTeamDTO> TeamWinners { get; set; }



    }
}