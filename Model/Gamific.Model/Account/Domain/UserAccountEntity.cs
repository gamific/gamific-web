using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Account.Domain
{
    /// <summary>
    /// Conta de usuário
    /// </summary>
    [Table("Account_UserAccount")]
    [DataContract]
    public class UserAccountEntity
    {
        /// <summary>
        /// Id da conta de usuario
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [IgnoreDataMember]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Selo de segurança
        /// </summary>
        [IgnoreDataMember]
        public string SecurityStamp { get; set; }

        [DataMember(Name = "status")]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "lockoutDate")]
        public DateTime? LockoutDate { get; set; }

        [DataMember(Name = "accessFailedCount")]
        public int AccessFailedCount { get; set; }

        [DataMember(Name = "LastUpdate")]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "lastLogin")]
        public DateTime LastLogin { get; set; }

        [DataMember(Name = "tokenMobile")]
        public string TokenMobile { get; set; }

        [DataMember(Name = "device")]
        public string Device { get; set; }


    }
}
