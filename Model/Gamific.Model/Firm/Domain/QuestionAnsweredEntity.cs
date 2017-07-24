using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Mapeia o questionário da empresa
    /// </summary>
    [Table("Firm_Question_Answered")]
    [DataContract]
    public class QuestionAnsweredEntity:GenericEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "IdQuestion")]
        [JsonProperty("idQuestion")]
        public int IdQuestion { get; set; }

        [Required]
        [DataMember(Name = "IdAnswers")]
        [JsonProperty("idAnswers")]
        public int IdAnswers { get; set; }

        [Required]
        [DataMember(Name = "userId")]
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [Required]
        [DataMember(Name = "lastUpdate")]
        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [Required]
        [DataMember(Name = "idQuiz")]
        [JsonProperty("idQuiz")]
        public int IdQuiz { get; set; }
        
        [Required]
        [DataMember(Name = "quizProcess")]
        [JsonProperty("quizProcess")]
        public string QuizProcess { get; set; }

        [Required]
        [DataMember(Name = "playerId")]
        [JsonProperty("playerId")]
        public string PlayerId { get; set; }
    }
}