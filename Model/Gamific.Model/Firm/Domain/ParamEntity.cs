using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    [Table("Firm_Param")]
    [DataContract]
    public class ParamEntity
    {
        [NotMapped]
        public static string GRAFICO_PRODUTOS = "GRAFICO_PRODUTOS";


        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        [Required]
        public string Name { get; set; }

        [DataMember(Name = "value")]
        [Required]
        public string Value { get; set; }

        [DataMember(Name = "description")]
        [Required]
        public string Description { get; set; }

        [DataMember(Name = "gameId")]
        public String GameId { get; set; }

        [DataMember(Name = "lastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "updateBy")]
        [Required]
        public int UpdateBy { get; set; }




    }
}