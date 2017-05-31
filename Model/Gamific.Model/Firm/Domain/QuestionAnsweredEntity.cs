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
    [Table("firm_question_answered")]
    [DataContract]
    public class QuestionAnsweredEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "IdQuestion")]
        public int IdQuestion { get; set; }

        [Required]
        [DataMember(Name = "IdAnswers")]
        public int IdAnswers { get; set; }

        [Required]
        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        [Required]
        [DataMember(Name = "lastUpdate")]
        public DateTime LastUpdate { get; set; }

    }
}