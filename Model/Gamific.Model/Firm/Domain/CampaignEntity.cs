using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// campanha
    /// </summary>
    [Table("Firm_Campaign")]
    [DataContract]
    public class CampaignEntity
    {
        /// <summary>
        /// Id da campanha
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "campaignName")]
        [Required]
        public string CampaignName { get; set; }

        [DataMember(Name = "description")]
        [Required]
        public string Description { get; set; }

        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }

        [DataMember(Name = "sponsorId")]
        public int SponsorId { get; set; }

        [DataMember(Name = "workerTypeId")]
        public int WorkerTypeId { get; set; }

        [DataMember(Name = "initialDate")]
        [Required]
        public DateTime InitialDate { get; set; }

        [DataMember(Name = "endDate")]
        [Required]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "status")]
        [Required]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CampaignEntity p = obj as CampaignEntity;
            if ((System.Object)p == null)
            {
                return false;
            }

            return p.Id == this.Id;
        }

    }
}