using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vlast.Broker.EMAIL;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Util.Parameter;

namespace Vlast.Gamific.Web.Controllers.Util
{
    public class EmailSender
    {
        public static void Send(EmailSupportDTO email, string emailTo, string gameId = "", string episodeId = "", string playerId = "")
        {
            string emailFrom = ParameterCache.Get("SUPPORT_EMAIL");
            bool result = EmailDispatcher.SendEmail(emailFrom, email.Subject, new List<string>() { emailTo }, email.Msg);
            EmailLogRepository.Instance.Create(new EmailLogEntity{
                Description = email.Subject,
                GameId = gameId, 
                EpisodeId = episodeId,
                PlayerId = playerId,
                To = emailTo,
                Message = email.Msg,
            });
        }
    }
}