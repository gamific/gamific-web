using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Vlast.Gamific.Model.Firm.Domain
{
    [Table("Firm_Result")]
    [DataContract]
    public class ResultEntity
    {
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "result")]
        [Required]
        public int Result { get; set; }

        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }

        [DataMember(Name = "workerId")]
        [Required]
        public int WorkerId { get; set; }

        [DataMember(Name = "metricId")]
        [Required]
        public int MetricId { get; set; }

        [NotMapped]
        public string MetricExternalId { get; set; }

        [DataMember(Name = "period")]
        [Required]
        public DateTime Period { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

        [DataMember(Name = "mainResult")]
        public int? MainResult { get; set; }

        [NotMapped]
        public string PeriodString { get; set; }
    }
}