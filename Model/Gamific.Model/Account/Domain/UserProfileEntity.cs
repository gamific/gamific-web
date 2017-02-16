using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Gamific.Model.Account.Domain
{
    /// <summary>
    /// Perfil do usuário
    /// </summary>
    [Table("Account_UserProfile")]
    [DataContract]
    public class UserProfileEntity
    {
        /// <summary>
        /// Id da conta de usuario
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        [Required]
        public string Name { get; set; }

        [DataMember(Name = "email")]
        [Required]
        public string Email { get; set; }

        [DataMember(Name = "phone")]
        [Required]
        public string Phone { get; set; }

        [DataMember(Name = "cpf")]
        public string CPF { get; set; }

        [DataMember(Name = "LastUpdate")]
        [Required]
        public DateTime LastUpdate { get; set; }

    }
}
