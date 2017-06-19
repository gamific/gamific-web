using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Vlast.Gamific.Model.Firm.Domain
{
    public class QuizCompleteDTO
    {

        public int Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public bool Required { get; set; }

        public int IdQuizQuestion { get; set; }

        public string CreatedBy { get; set; }

        public DateTime InitialDate { get; set; }

        public int Score { get; set; }

        public bool IsMultiple { get; set; }

        public DateTime LastUpdate { get; set; }

        public string UpdatedBy { get; set; }

        public string Link { get; set; }

        public int FirmId { get; set; }

        public DateTime? DateLimit { get; set; }

        public bool status { get; set; }

        public List<QuestionDTO> questions { get; set; }

    }
}