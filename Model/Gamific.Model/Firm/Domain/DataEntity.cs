using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// dados da empresa
    /// </summary>
    [Table("Firm_Data")]
    [DataContract]
    public class DataEntity
    {
        /// <summary>
        /// Id da empresa 
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// FirmName
        /// </summary>
        [DataMember(Name = "firmName")]
        [Required(ErrorMessage = "O nome da empresa é obrigatório.")]
        public string FirmName { get; set; }

        /// <summary>
        /// Razao Social
        /// </summary>
        [DataMember(Name = "companyName")]
        [Required(ErrorMessage = "A razão social é obrigatória.")]
        public string CompanyName { get; set; }

        /// <summary>
        /// CNPJ
        /// </summary>
        [DataMember(Name = "cnpj")]
        [Required(ErrorMessage = "O cnpj é obrigatório.")]
        public string Cnpj { get; set; }

        /// <summary>
        /// id da logo na aws
        /// </summary>
        [DataMember(Name = "logoId")]
        public int LogoId { get; set; }

        /// <summary>
        /// Endreço
        /// </summary>
        [DataMember(Name = "adress")]
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Adress { get; set; }

        /// <summary>
        /// Bairro
        /// </summary>
        [DataMember(Name = "neighborhood")]
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Neighborhood { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [DataMember(Name = "city")]
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string City { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        [DataMember(Name = "phone")]
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Phone { get; set; }

        [DataMember(Name = "status")]
        [Required]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }

    }
}