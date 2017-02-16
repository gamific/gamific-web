using Vlast.Gamific.Account;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe com informações para edição de uma equipe
    /// </summary>
    public class TeamDTO
    {
        public string TeamName { get; set; }

        public int IdTeam { get; set; }

        public int LogoId { get; set; }

        //ID da associação entre equipe e campanha
        public int IdAssociation { get; set; }

        public string SponsorName { get; set; }

        public string ProfileName { get; set; }

        public int SponsorId { get; set; }
    }
}
