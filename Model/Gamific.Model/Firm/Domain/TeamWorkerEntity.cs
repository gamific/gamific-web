using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    [Table("Firm_Team_Worker")]
    [DataContract]
    public class TeamWorkerEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }

        [DataMember(Name = "workerId")]
        public int? WorkerId { get; set; }

        [DataMember(Name = "teamId")]
        public int? TeamId { get; set; }

        [DataMember(Name = "externalTeamId")]
        public string ExternalTeamId { get; set; }

        [DataMember(Name = "externalWorkerId")]
        public string ExternalWorkerId { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

        [DataMember(Name = "status")]
        [Required]
        public GenericStatus Status { get; set; }

        [NotMapped]
        public bool selected { get; set; }

    }
}