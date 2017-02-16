using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Public.Domain
{
    /// <summary>
    /// topico de ajuda
    /// </summary>
    [Table("Public_TopicHelp")]
    [DataContract]
    public class TopicHelpEntity
    {
        /// <summary>
        /// Id do topico da ajuda
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
        /// Nome topico
        /// </summary>
        [DataMember(Name = "topicName")]
        [Required(ErrorMessage = "O nome do tópico é obrigatório.")]
        public string TopicName { get; set; }

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