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
    [Table("firm_question_answers")]
    [DataContract]
    public class QuestionAnswersEntity : GenericEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "IdQuestion")]
        public int IdQuestion { get; set; }

        [Required]
        [DataMember(Name = "IdAnswer")]
        public int IdAnswer { get; set; }

        [DataMember(Name = "isRight")]
        public bool IsRight { get; set; }

        [DataMember(Name = "ordination")]
        public int Ordination { get; set; }


    }
}