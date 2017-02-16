using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    public class MetricDTO
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "metricName")]
        public string MetricName { get; set; }

        [DataMember(Name = "minValue")]
        public int? MinValue { get; set; }

        [DataMember(Name = "icon")]
        [Required]
        public string Icon { get; set; }

        [DataMember(Name = "valueMax")]
        public int? ValueMax { get; set; }

        [DataMember(Name = "weigth")]
        [Required]
        public int Weigth { get; set; }

        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "status")]
        [Required]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

    }
}