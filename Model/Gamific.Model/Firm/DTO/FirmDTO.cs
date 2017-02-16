using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe com informações para edição de uma empresa
    /// </summary>
    public class FirmDTO
    {
        public FirmDTO()
        {
            DataInfo = new DataEntity();
            ProfileInfo = new UserProfileEntity();
        }

        public DataEntity DataInfo { get; set; }

        public UserProfileEntity ProfileInfo { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public int Status { get; set; }
    }
}
