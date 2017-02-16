using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Pergunta Resposta
    /// </summary>
    [Table("Firm_Video_Question_Answered")]
    [DataContract]
    public class VideoQuestionAnsweredEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "videoQuestionId")]
        [Required]
        public int VideoQuestionId { get; set; }

        [DataMember(Name = "answer")]
        [Required]
        public string Answer { get; set; }

        [DataMember(Name = "userId")]
        [Required]
        public int UserId { get; set; }

        [DataMember(Name = "answeredDate")]
        [Required]
        public DateTime AnsweredDate { get; set; }

    }
}