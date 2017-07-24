using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Gamific.Model.Firm.DTO
{
    public class AnswersDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("idQuestion")]
        public int IdQuestion { get; set; }

        [JsonProperty("isCorrect")]
        public bool IsCorrect { get; set; }
    }
}
