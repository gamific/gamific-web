using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;


namespace Vlast.Gamific.Model.Firm.DTO
{
    public class ReportDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string GameName { get; set; }
        public DateTime LastUpdateWeb { get; set; }
        public DateTime LastUpdateMobile { get; set; }
        public DateTime LastReciveEmail { get; set; }
        public string LastUpdateWebString { get; set; }
        public string LastUpdateMobileString { get; set; }
        public string LastReciveEmailString { get; set; }
        public int CountEmails { get; set; }
    }
}
