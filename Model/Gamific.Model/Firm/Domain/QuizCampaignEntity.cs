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
    [Table("Firm_Quiz_Campaign")]
    [DataContract]
    public class QuizCampaignEntity : GenericEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DataMember(Name = "idQuiz")]
        public int IdQuiz { get; set; }

        [DataMember(Name = "IdCampaign")]
        public string IdCampaign { get; set; }

    }
    }