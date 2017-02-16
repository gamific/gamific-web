using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// equipe
    /// </summary>
    [Table("Firm_Team")]
    [DataContract]
    public class TeamEntity
    {
        /// <summary>
        /// Id do equipe
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// id da logo na aws
        /// </summary>
        [DataMember(Name = "logoId")]
        [Required(ErrorMessage = "A logo é obrigatória.")]
        public int LogoId { get; set; }

        /// <summary>
        /// id externo
        /// </summary>
        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Nome equipe
        /// </summary>
        [DataMember(Name = "teamName")]
        [Required(ErrorMessage = "O nome da equipe é obrigatório.")]
        public string TeamName { get; set; }

        /// <summary>
        /// Empresa
        /// </summary>
        [DataMember(Name = "firmId")]
        [Required(ErrorMessage = "A empresa é obrigatória.")]
        public int FirmId { get; set; }

        /// <summary>
        /// responsavel
        /// </summary>
        [DataMember(Name = "sponsorId")]
        public int SponsorId { get; set; }

        /// <summary>
        /// tipo funcionario
        /// </summary>
        [DataMember(Name = "workerTypeId")]
        public int WorkerTypeId { get; set; }

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

            TeamEntity p = obj as TeamEntity;
            if ((System.Object)p == null)
            {
                return false;
            }

            return p.Id == this.Id;
        }

    }
}