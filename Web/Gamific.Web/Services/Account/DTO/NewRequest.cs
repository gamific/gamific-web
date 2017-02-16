using System.Runtime.Serialization;

namespace Vlast.Gamific.Api.Account.Dto
{
    /// <summary>
    /// Dados para criação de um novo usuário
    /// </summary>
    [DataContract]
    public class NewRequest
    {
        [DataMember(Name = "firmId")]
        public int FirmId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "usuario")]
        public string Username { get; set; }

        [DataMember(Name = "cpf")]
        public string Cpf { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

    }
}