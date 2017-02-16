using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Video
    /// </summary>
    [Table("Firm_Video")]
    [DataContract]
    public class VideoEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "videoUrl")]
        [Required]
        public string VideoUrl { get; set; }

        [DataMember(Name = "videoTitle")]
        [Required]
        public string VideoTitle { get; set; }

        [DataMember(Name = "status")]
        [Required]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

    }
}