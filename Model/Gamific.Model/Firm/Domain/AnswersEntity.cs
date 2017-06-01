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
    [Table("Firm_Answer")]
    [DataContract]
    public class AnswersEntity : GenericEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "answer")]
        public String Answer { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public String Name { get; set; }


        [DataMember(Name = "firmId")]
        public int FirmId { get; set; }

        [DataMember(Name = "status")]
        public bool status { get; set; }

        [DataMember(Name = "lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        public string UpdatedBy { get; set; }

        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "initialDate")]
        public DateTime InitialDate { get; set; }

    }
}