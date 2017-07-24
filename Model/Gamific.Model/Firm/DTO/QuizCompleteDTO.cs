using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    public class QuizCompleteDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("idQuizQuestion")]
        public int IdQuizQuestion { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("initialDate")]
        public DateTime InitialDate { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("isMultiple")]
        public bool IsMultiple { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonIgnore]
        public int FirmId { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("dateLimit")]
        public DateTime? DateLimit { get; set; }

        [JsonIgnore]
        public bool status { get; set; }

        [JsonProperty("questions")]
        public List<QuestionDTO> questions { get; set; }

    }
}