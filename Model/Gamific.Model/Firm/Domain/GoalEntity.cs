using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Meta de um funcionario para uma metrica
    /// </summary>
    [Table("Firm_Goal")]
    [DataContract]
    public class GoalEntity
    {
        /// <summary>
        /// Id do worker_type_metric
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// External MetricId Engine
        /// </summary>
        [DataMember(Name = "externalMetricId")]
        public string ExternalMetricId { get; set; }

        [DataMember(Name = "runId")]
        [Required]
        public string RunId { get; set; }

        [DataMember(Name = "episodeId")]
        public string EpisodeId { get; set; }

        [DataMember(Name = "goal")]
        [Required(ErrorMessage = "A meta é obrigatoria.")]
        public int Goal { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

        //sobescrever contains

    }
}
