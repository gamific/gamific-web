using System.Runtime.Serialization;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe com informações do ranking
    /// </summary>
    public class RankingDTO
    {
        [DataMember(Name = "playerName")]
        public string PlayerName { get; set; }

        [DataMember(Name = "playerId")]
        public string PlayerId { get; set; }

        [DataMember(Name = "logoId")]
        public int LogoId { get; set; }

        [DataMember(Name = "score")]
        public int? Score { get; set; }
    }
}
