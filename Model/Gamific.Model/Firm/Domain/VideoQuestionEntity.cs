using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Pergunta do video
    /// </summary>
    [Table("Firm_Video_Question")]
    [DataContract]
    public class VideoQuestionEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "videoId")]
        [Required]
        public int VideoId { get; set; }

        [DataMember(Name = "questionName")]
        [Required]
        public string QuestionName { get; set; }

        [DataMember(Name = "correctAnswer")]
        [NotMapped]
        public string CorrectAnswer { get; set; }

        [DataMember(Name = "answers")]
        [NotMapped]
        public string Answers { get; set; }

        [DataMember(Name = "firmId")]
        [Required]
        public int FirmId { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        [Required]
        public int UpdatedBy { get; set; }

        [DataMember(Name = "status")]
        [Required]
        public GenericStatus Status { get; set; }

    }
}