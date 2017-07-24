using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Mapeia o questionário da empresa
    /// </summary>
    [Table("Firm_Quiz")]
    [DataContract]
    public class QuizEntity : GenericEntity
    {

        public QuizEntity() { }

        public QuizEntity(int id, string name, string description, bool required, int idQuizQuestion, string createdBy,
        DateTime initialDate, int score, bool isMultiple, DateTime lastUpdate,
        string updatedBy, int firmId)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Required = required;
            this.CreatedBy = createdBy;
            this.IdQuizQuestion = idQuizQuestion;
            this.InitialDate = initialDate;
            this.Score = score;
            this.IsMultiple = isMultiple;
            this.LastUpdate = lastUpdate;
            this.UpdatedBy = updatedBy;
            this.FirmId = firmId;
        }


        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "description")]
        public String Description { get; set; }

        [DataMember(Name = "required")]
        public bool Required { get; set; }

        [DataMember(Name = "idQuizQuestion")]
        public int IdQuizQuestion { get; set; }

        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "initialDate")]
        public DateTime InitialDate { get; set; }

        [DataMember(Name = "score")]
        public int Score { get; set; }

        [DataMember(Name = "isMultiple")]
        public bool IsMultiple { get; set; }

        [DataMember(Name = "lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updatedBy")]
        public string UpdatedBy { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        [DataMember(Name = "firmId")]
        public int FirmId { get; set; }

        [DataMember(Name = "dateLimit")]
        public DateTime? DateLimit { get; set; }

        [DataMember(Name = "status")]
        public bool status { get; set; }

        [DataMember(Name = "gameId")]
        public string GameId { get; set; }
    }
}