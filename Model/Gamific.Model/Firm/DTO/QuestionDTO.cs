using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    public class QuestionDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("question")]
        public String Question { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("idQuestionAnswered")]
        public int? IdQuestionAnswered { get; set; }

        [JsonProperty("idQuestionAnswers")]
        public int? IdQuestionAnswers { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("initialDate")]
        public DateTime InitialDate { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty("link")]
        public String Link { get; set; }

        [JsonIgnore]
        public bool status { get; set; }

        [JsonIgnore]
        public int FirmId { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("answers")]
        public List<AnswersDTO> answers { get; set; }
    }
}