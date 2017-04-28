using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Account.Domain
{
    [Table("Account_Devices")]
    [DataContract]
    public class AccountDevicesEntity
    {
        [Key]
        [DataMember(Name = "ID")]
        public string Id { get; set; }

        [DataMember(Name = "UUID")]
        public string UUID { get; set; }

        [DataMember(Name = "NOTIFICATION_TOKEN")]
        public string Notification_Token { get; set; }

        [DataMember(Name = "PLATAFORM")]
        public string Plataform { get; set; }

        [DataMember(Name = "EXTERNAL_USER_ID")]
        public string External_User_Id { get; set; }

        [DataMember(Name = "LAST_UPDATE")]
        public DateTime Last_Update { get; set; }

        //[NotMapped]
        //public string PlayerName { get; set; }

    }
}
