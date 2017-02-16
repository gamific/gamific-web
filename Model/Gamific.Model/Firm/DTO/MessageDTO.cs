using System;
using System.Runtime.Serialization;
using Vlast.Gamific.Account;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe com informações das mensagens
    /// </summary>
    public class MessageDTO
    {
        public string SenderName { get; set; }

        public string SendDateTime { get; set; }

        public string Message { get; set; }

        public int SenderLogoId { get; set; }

        public int Id { get; set; }

        public int FirmId { get; set; }

        public int Sender { get; set; }

        public string TeamId { get; set; }

    }
}
