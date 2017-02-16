using System.Runtime.Serialization;

namespace Vlast.Gamific.Model.Account.DTO
{
    public enum Roles
    {
        [EnumMember]
        ADMINISTRATOR, //0
        [EnumMember]
        WORKER // 1 Funcionário da empresa
    }
}
