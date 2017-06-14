using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class QuizCompleteDTO
    {
        [JsonProperty("QuestionEntity")]
        public QuestionEntity QuestionEntity { get; set; }

        [JsonProperty("AnswersEntity")]
        public List<AnswersEntity> AnswersEntity { get; set; }

        [JsonProperty("QuizEntity")]
        public QuizEntity QuizEntity { get; set; }


    }

}