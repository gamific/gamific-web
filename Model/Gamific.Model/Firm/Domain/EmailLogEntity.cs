using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Gamific.Model.Firm.Domain
{
    [Table("Email_Log")]
    [DataContract]
    public class EmailLogEntity
    {
        [Key]
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "To")]
        public string To { get; set; }

        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "SendTime")]
        public DateTime SendTime { get; set; }

        [DataMember(Name = "GameId")]
        public string GameId { get; set; }

        [DataMember(Name = "EpisodeId")]
        public string EpisodeId { get; set; }

        [DataMember(Name = "PlayerId")]
        public string PlayerId { get; set; }
    }
}
