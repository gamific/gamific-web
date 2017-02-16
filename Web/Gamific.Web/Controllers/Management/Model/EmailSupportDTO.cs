namespace Vlast.Gamific.Web.Controllers.Management.Model
{
    /// <summary>
    /// Classe com informações para envio de email de suporte
    /// </summary>
    public class EmailSupportDTO
    {
        public string Category { get; set; }

        public string Subject { get; set; }

        public string Msg { get; set; }
    }
}
