using System.Runtime.Serialization;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe com informações da equipe
    /// </summary>
    public class EngineTeamDTO
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "idExterno")]
        public string IdExterno { get; set; }

        [DataMember(Name = "score")]
        public int? Score { get; set; }
    }
}
