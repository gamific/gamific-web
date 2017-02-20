using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class PlayerEngineDTO
    {
        public PlayerEngineDTO() { }

        public PlayerEngineDTO(string gameId,string nick, string role, int? level, int logoId)
        {
            this.Nick = nick;
            this.Role = role;
            this.Level = level;
            this.GameId = gameId;
            this.LogoId = logoId;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("xp")]
        public int? Xp { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("logoId")]
        public int LogoId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("cpf")]
        public string Cpf { get; set; }
    
        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}