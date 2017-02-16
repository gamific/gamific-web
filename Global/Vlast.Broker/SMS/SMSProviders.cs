 
 
using HumanAPIClient.Model;
using HumanAPIClient.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vlast.Util.Instrumentation;

namespace Vlast.Broker.SMS
{
    public abstract class SMSProvider
    {
        public abstract bool SendSMS(string msg, string phone);
        protected string ParsePhone(string phone)
        {
            string refPhone = phone;

            //Removendo prefixo 55 ou 550 (no caso de DDD comecando em 0)
            if (refPhone.Length > 12 && refPhone.StartsWith("550"))
                refPhone = refPhone.Substring(3);
            else if (refPhone.Length > 11 && refPhone.StartsWith("55"))
                refPhone = refPhone.Substring(2);

            return refPhone;
        }
    }

    /// <summary>
    /// Provedor de SMS
    /// http://locasms.com.br/
    /// </summary>
    public class LocaSMSProvider : SMSProvider
    {
        private static string SERVICE_URL = "http://173.44.33.18/painel/api.ashx?action=sendsms&lgn=mypush&pwd=C3dr0MyPu5h@5M5&&msg={0}&numbers={1}";

        /// <summary>
        /// Envia um SMS via http://locasms.com.br
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public override bool SendSMS(string msg, string phone)
        {
            bool result = false;

            try
            {
                phone = phone.Replace("+","");
                if (phone.StartsWith("55"))
                {
                    phone = phone.Substring(2,phone.Length-2);
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(SERVICE_URL, msg, phone));
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }
    }


    /// <summary>
    /// Provedor de SMS
    /// http://api.mundotelecom.com.br/
    /// </summary>
    public class MundoTelecomSMSProvider : SMSProvider
    {
        private static string SERVICE_URL = "";

        /// <summary>
        /// Envia um SMS via http://locasms.com.br
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public override bool SendSMS(string msg, string phone)
        {
            bool result = false;

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(SERVICE_URL, phone, msg));
                request.Method = "POST";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }
    }
 
    public class ZenviaSMSProvider : SMSProvider
    {
        private static string account = "";
        private static string code = "MRSHEiUF06";

        /// <summary>
        /// Envia um SMS via Zenvia
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public override bool SendSMS(string msg, string phone)
        {
            bool result = false;

            try
            {
                SimpleSending simpleSending = new SimpleSending(account, code);

                SimpleMessage message = new SimpleMessage();
                message.To = phone;
                message.Message = msg;

                List <string> responses = simpleSending.send(message);

                foreach (string response in responses)
                {
                    if (response == null)
                        continue;

                    //000 - Message Sent
                    string [] responseParts = response.Split('-');

                    if (responseParts != null && responseParts.Length > 1 && responseParts[0].Trim().Equals("000"))
                    {
                        result = true;
                        break;
                    }

                    Logger.LogInfo(response);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }
    }
}
