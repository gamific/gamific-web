using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Mapeia o funcionário da empresa
    /// </summary>
    [Table("Firm_Worker")]
    [DataContract]
    public class WorkerEntity
    {
        /// <summary>
        /// Id do funcionário
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// id da logo na aws
        /// </summary>
        [DataMember(Name = "logoId")]
        public int LogoId { get; set; }

        /// <summary>
        /// id externo
        /// </summary>
        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Empresa
        /// </summary>
        [DataMember(Name = "firmId")]
        [Required(ErrorMessage = "A empresa é obrigatória.")]
        public int FirmId { get; set; }

        /// <summary>
        /// Empresa
        /// </summary>
        [DataMember(Name = "externalFirmId")]
        public string ExternalFirmId { get; set; }

        /// <summary>
        /// User Profile
        /// </summary>
        [DataMember(Name = "userProfileId")]
        [Required(ErrorMessage = "O usuario é obrigatório.")]
        public int UserId { get; set; }

        /// <summary>
        /// tipo funcionario
        /// </summary>
        [DataMember(Name = "workerTypeId")]
        [Required(ErrorMessage = "O perfil é obrigatório.")]
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
    }
}