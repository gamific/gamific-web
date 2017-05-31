using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class QuizDTO
    {
        public QuizDTO() { }

        public QuizDTO(int? id, string name, string description, bool? required, int? idQuizQuestion, string createdBy,
        DateTime? initialDate, int? ordination, string quizProcess, int? score, bool? isMultiple, DateTime? lastUpdate,
        string updatedBy, int firmId, DateTime? DateLimit, bool status)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Required = required;
            this.CreatedBy = createdBy;
            this.IdQuizQuestion = idQuizQuestion;
            this.InitialDate = initialDate;
            this.Ordination = ordination;
            this.QuizProcess = quizProcess;
            this.Score = score;
            this.IsMultiple = isMultiple;
            this.LastUpdate = lastUpdate;
            this.UpdatedBy = updatedBy;
            this.FirmId = firmId;
            this.DateLimit = DateLimit;
            this.status = status;
        }


        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("required")]
        public bool? Required { get; set; }

        [JsonProperty("idQuizQuestion")]
        public int? IdQuizQuestion { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("initialDate")]
        public DateTime? InitialDate { get; set; }

        [JsonProperty("ordination")]
        public int? Ordination { get; set; }

        [JsonProperty("quizProcess")]
        public String QuizProcess { get; set; }

        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("isMultiple")]
        public bool? IsMultiple { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty("firmId")]
        public int? FirmId { get; set; }

        [JsonProperty("qtdPerguntas")]
        public int? QtdPerguntas { get; set; }

        [JsonProperty("dateLimit")]
        public DateTime? DateLimit { get; set; }

        [JsonProperty("status")]
        public bool? status { get; set; }
    }
}