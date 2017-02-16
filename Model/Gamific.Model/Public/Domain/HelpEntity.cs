using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Public.Domain
{
    /// <summary>
    /// ajuda
    /// </summary>
    [Table("Public_Help")]
    [DataContract]
    public class HelpEntity
    {
        /// <summary>
        /// Id da ajuda
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Empresa
        /// </summary>
        [DataMember(Name = "firmId")]
        [Required(ErrorMessage = "A empresa é obrigatória.")]
        public int FirmId { get; set; }

        /// <summary>
        /// Topico
        /// </summary>
        [DataMember(Name = "topicId")]
        [Required(ErrorMessage = "O tópico é obrigatório.")]
        public int TopicId { get; set; }

        /// <summary>
        /// Conteudo ajuda
        /// </summary>
        [DataMember(Name = "helpContent")]
        [Required(ErrorMessage = "O conteúdo da ajuda é obrigatório.")]
        public string HelpContent { get; set; }

        /// <summary>
        /// Titulo ajuda
        /// </summary>
        [DataMember(Name = "helpTitle")]
        [Required(ErrorMessage = "O titulo da ajuda é obrigatório.")]
        public string HelpTitle { get; set; }

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