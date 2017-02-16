using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Mensagem
    /// </summary>
    [Table("Firm_Message")]
    [DataContract]
    public class MessageEntity
    {
        /// <summary>
        /// Id da mensagem
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// id da empresa
        /// </summary>
        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }

        /// <summary>
        /// id de quem esta enviando a mensagem
        /// </summary>
        [DataMember(Name = "sender")]
        [Required]
        public int Sender { get; set; }

        /// <summary>
        /// id da equipe
        /// </summary>
        [DataMember(Name = "teamId")]
        [Required]
        public string TeamId { get; set; }

        /// <summary>
        /// Mensagem
        /// </summary>
        [DataMember(Name = "message")]
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// Data de envio
        /// </summary>
        [DataMember(Name = "sendDateTime")]
        [Required]
        public DateTime SendDateTime { get; set; }

    }
}