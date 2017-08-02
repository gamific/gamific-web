using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    [Table("Firm_Question_Answered_Selected")]
    [DataContract]
    public class QuestionAnsweredSelectedEntity : GenericEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DataMember(Name = "answerId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("answerId")]
        public int AnswerId { get; set; }

        [DataMember(Name = "quizId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("quizId")]
        public int QuizId { get; set; }

        [DataMember(Name = "playerId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("playerId")]
        public string PlayerId { get; set; }

        [DataMember(Name = "questionId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("questionId")]
        public int QuestionId { get; set; }
    }
}
