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
        public int IdQuestion { get; set; }
        public int IdAnswers { get; set; }
        public int UserId { get; set; }
        public int IdQuiz { get; set; }
    }
}