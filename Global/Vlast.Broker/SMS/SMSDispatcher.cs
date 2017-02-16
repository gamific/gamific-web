using Vlast.Util.Instrumentation;
using Vlast.Util.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Broker.SMS
{
    public class SMSDispatcher
    {
        /// <summary>
        /// Lista dos serviços SMS disponíveis
        /// </summary>
        private static List<SMSProvider> _providers;
        public static List<SMSProvider> Providers
        {
            get
            {
                if (_providers == null)
                {
                    //Ordem de disparo
                    _providers = new List<SMSProvider>();
                    _providers.Add(new ZenviaSMSProvider());
                    
                    _providers.Add(new MundoTelecomSMSProvider());
                    //_providers.Add(new LocaSMSProvider());
                }
                return _providers;
            }
        }

        /// <summary>
        /// Envia sms tentando mandar para os serviços disponíveis
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static async Task<bool> SendSMS(string msg, string phone)
        {
            bool sent = false;
            try
            {
                foreach (SMSProvider provider in Providers)
                {
                    //Manda pelo primeiro que der certo
                    if (provider.SendSMS(msg, phone))
                    {
                        sent = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                sent = false;
                Logger.LogException(ex);
            }
            return sent;
        }
    }
}
