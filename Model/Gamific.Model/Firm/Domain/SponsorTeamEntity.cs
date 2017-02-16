using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Domain
{
    /// <summary>
    /// Relaçao entre responsavel e equipe
    /// </summary>
    [Table("Firm_Sponsor_Team")]
    [DataContract]
    public class SponsorTeamEntity
    {
        /// <summary>
        /// Id 
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Id do responsavel
        /// </summary>
        [DataMember(Name = "sponsorId")]
        public string SponsorId { get; set; }

        /// <summary>
        /// Id da equipe
        /// </summary>
        [DataMember(Name = "teamId")]
        public string TeamId { get; set; }
    }
}
