using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    public class QuestionAnsweredDTO
    {
        [JsonProperty("idQuestion")]
        public int IdQuestion { get; set; }

        [JsonProperty("idAnswer")]
        public int IdAnswers { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonProperty("idQuiz")]
        public int IdQuiz { get; set; }

        [JsonProperty("playerId")]
        public string PlayerId { get; set; }
    }
}