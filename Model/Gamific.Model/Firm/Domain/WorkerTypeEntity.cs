using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// tipo de funcionario
    /// </summary>
    [Table("Firm_Worker_Type")]
    [DataContract]
    public class WorkerTypeEntity
    {

        /// <summary>
        /// Id do tipo jogador
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nome perfil
        /// </summary>
        [DataMember(Name = "profileName")]
        [Required(ErrorMessage = "O nome do perfil é obrigatório.")]
        public Profiles ProfileName { get; set; }

        /// <summary>
        /// Nome tipo jogador
        /// </summary>
        [DataMember(Name = "typeName")]
        [Required(ErrorMessage = "O nome do tipo de jogador é obrigatório.")]
        public string TypeName { get; set; }

        /// <summary>
        /// Empresa
        /// </summary>
        [DataMember(Name = "firmId")]
        [Required(ErrorMessage = "A empresa é obrigatória.")]
        public long FirmId { get; set; }

        /// <summary>
        /// Empresa
        /// </summary>
        [DataMember(Name = "externalFirmId")]
        public string ExternalFirmId { get; set; }

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