using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Gamific.Model.Account.DTO;

namespace Vlast.Gamific.Model.Account.Domain
{
    /// <summary>
    /// Mapeia usuário e suas permissões
    /// </summary>
    [Table("Account_UserRole")]
    [DataContract]
    public class UserRoleEntity
    {
        /// <summary>
        /// Id da conta de usuario
        /// </summary>
        [Key]
        [DataMember(Name = "userId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserId { get; set; }

        /// <summary>
        /// User Role
        /// </summary>
        [DataMember(Name = "role")]
        public Roles Role { get; set; }
    }
}
