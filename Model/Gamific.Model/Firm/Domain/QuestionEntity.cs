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
    [Table("Firm_Question")]
    [DataContract]
    public class QuestionEntity : GenericEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "question")]
        public String Question { get; set; }

        [DataMember(Name = "required")]
        public bool Required { get; set; }

        [DataMember(Name = "idQuestionAnswered")]
        public int? IdQuestionAnswered { get; set; }

        [DataMember(Name = "idQuestionAnswers")]
        public int? IdQuestionAnswers { get; set; }

        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "initialDate")]
        public DateTime InitialDate { get; set; }

        [DataMember(Name = "lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        public string UpdatedBy { get; set; }

        [DataMember(Name = "link")]
        public String Link { get; set; }

        [DataMember(Name = "status")]
        public bool status { get; set; }

        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }
    }
}