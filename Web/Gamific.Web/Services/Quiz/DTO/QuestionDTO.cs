using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        public String Question { get; set; }

        public bool Required { get; set; }

        public int? IdQuestionAnswered { get; set; }

        public int? IdQuestionAnswers { get; set; }

        public string CreatedBy { get; set; }

        public DateTime InitialDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public string UpdatedBy { get; set; }

        public String Link { get; set; }

        public bool status { get; set; }

        public int FirmId { get; set; }

        public List<AnswersEntity> answers { get; set; }
    }
}