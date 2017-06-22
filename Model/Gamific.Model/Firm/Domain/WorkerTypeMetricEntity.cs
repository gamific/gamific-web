using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Ligação entre tipo de funcionario e metrica
    /// </summary>
    [Table("Firm_Worker_Type_Metric")]
    [DataContract]
    public class WorkerTypeMetricEntity
    {
        /// <summary>
        /// Id do worker_type_metric
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// FK tipo jogador
        /// </summary>
        [DataMember(Name = "workerTypeId")]
        [Required(ErrorMessage = "O tipo de jogador é obrigatorio.")]
        public int WorkerTypeId { get; set; }

        /// <summary>
        /// FK Metric
        /// </summary>
        [DataMember(Name = "metricId")]
        public long MetricId { get; set; }

        [DataMember(Name = "metricExternalId")]
        [Required]
        public string MetricExternalId { get; set; }

        [DataMember(Name = "status")]
        [Required]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }
    }
}