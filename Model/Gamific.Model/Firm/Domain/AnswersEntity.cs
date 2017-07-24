using Newtonsoft.Json;
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
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [DataMember(Name = "answer")]
        [JsonProperty("answer")]
        public String Answer { get; set; }

        [Required]
        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public String Name { get; set; }

        [DataMember(Name = "firmId")]
        [JsonIgnore]
        public int FirmId { get; set; }

        [DataMember(Name = "status")]
        [JsonIgnore]
        public bool status { get; set; }

        [DataMember(Name = "lastUpdate")]
        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }

        [DataMember(Name = "createdBy")]
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "initialDate")]
        [JsonProperty("initialDate")]
        public DateTime InitialDate { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int IdAssociate { get; set; }
    }
}