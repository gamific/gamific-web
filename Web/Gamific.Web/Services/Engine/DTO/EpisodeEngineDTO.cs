using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class EpisodeEngineDTO
    {
        public EpisodeEngineDTO() { }

        public EpisodeEngineDTO(string episodeId, string gameId)
        {
            this.GameId = gameId;
            this.Id = episodeId;
        }

        public EpisodeEngineDTO(string id)
        {
            this.Id = id;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")] //
        [JsonProperty("name")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "O nome da Skin é obrigatório.")] 
        [JsonProperty("skin")]
        public string Skin { get; set; }

        [Required] //(ErrorMessage = "O Xp é obrigatório.")
        [JsonProperty("xpReward")]
        public int? XpReward { get; set; }

        [Required(ErrorMessage = "A data inicial é obrigatório.")]
        [JsonProperty("initDate")]
        public long initDate { get; set; }

        [Required(ErrorMessage = "A data final é obrigatório.")]
        [JsonProperty("finishDate")]
        public long finishDate { get; set; }

        public DateTime initDateAux { get; set; }


        public DateTime finishDateAux { get; set; }

        [JsonProperty("sendEmail")]
        public bool sendEmail { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }

        [JsonProperty("checked")]
        public bool checkedFlag { get; set; }
    }
}