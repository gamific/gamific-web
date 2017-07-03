using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;


namespace Vlast.Gamific.Model.Firm.DTO
{
    public class ReportDTO
    {
        //[DataMember(Name = "name")]
        //[Required]
        public string Name { get; set; }

        //[DataMember(Name = "email")]
        //[Required]
        public string Email { get; set; }

        //[JsonProperty("name")]
        public string GameName { get; set; }

        //[DataMember(Name = "LastUpdate")]
        public DateTime LastUpdateWeb { get; set; }

        //[DataMember(Name = "LAST_UPDATE")]
        public DateTime LastUpdateMobile { get; set; }

        //[DataMember(Name = "LastUpdate")]
        public string LastUpdateWebString { get; set; }

        //[DataMember(Name = "LAST_UPDATE")]
        public string LastUpdateMobileString { get; set; }


    }
}
