namespace Vlast.Gamific.Model.Account.DTO
{
    /// <summary>
    /// Classe com informações para edição de senha
    /// </summary>
    public class PasswordDTO
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordConfirmation { get; set; }

        public int UserId { get; set; }

    }
}
