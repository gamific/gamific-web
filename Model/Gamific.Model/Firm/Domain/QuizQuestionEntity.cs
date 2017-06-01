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
    [Table("Firm_Quiz_Question")]
    [DataContract]
    public class QuizQuestionEntity : GenericEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DataMember(Name = "idQuiz")]
        public int IdQuiz { get; set; }

        [DataMember(Name = "idQuestion")]
        public int IdQuestion { get; set; }

        [DataMember(Name = "ordination")]
        public int Ordination { get; set; }
    }

    }