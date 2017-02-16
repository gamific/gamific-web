using Amazon.SimpleNotificationService.Model;

using Vlast.Util.Aws;
using Vlast.Util.Parameter;
using System;
using Vlast.Broker.Properties;
using Vlast.Broker.Client;
using Vlast.Broker.SNS.Model;

namespace Vlast.Broker.Platform.Microsoft
{
    /// <summary>
    /// Configuração de notificação para Apple Notification service usando o AWS SNS
    /// </summary>
    public class APNSDispatcher : SNSDispatcher
    {
        #region Configuração

        private static volatile APNSDispatcher instance;
        private static object syncRoot = new Object();

        private APNSDispatcher()
        {
        }

        public static APNSDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new APNSDispatcher();
                    }
                }

                return instance;
            }
        }



        /// <summary>
        /// Retorna o endpoint de notificação para plataforma Microsoft
        /// </summary>
        internal static string SNS_APP_ARN
        {
            get
            {
                return ParameterCache.Get("SNS_APP_IOS_NAME");
            }
        }

        #endregion

        /// <summary>
        /// Envia um notification push para um device usando o sistema de notificações da Apple
        /// </summary>
        /// <param name="deviceEndpoint">ARN registrado no amazon</param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        protected override bool SendMessage(DeviceEntity device, string title, string message, bool silent ,string extra = "")
        {
            string msg = "{ " +  GetMessage(title, message, silent, extra) + " } ";

            PublishRequest snsRequest = new PublishRequest()
            {
                Message = msg,
                MessageStructure = "json",
                TargetArn = device.BrokerEndpoint
            };

            PublishResponse response = AWSFactory.SNSClient.Publish(snsRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Formata a mensagem para APNS
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public override string GetMessage(string title, string message, bool silent,string extra = "")
        {
            message = (message.Length > 140 ? message.Substring(0, 140) : message);
            message = !string.IsNullOrWhiteSpace(message) ? message + "..." : message;

            

            string apnsMsg = Resources.APNS_MSG.Replace("##MSG##", message);
            apnsMsg = apnsMsg.Replace("##TITLE##", title);
            apnsMsg = apnsMsg.Replace("##EXTRA##", extra);

            if (silent)
            {
                apnsMsg = apnsMsg.Replace("##SOUND##", "");
                apnsMsg = apnsMsg.Replace("##SILENT##", "1");
            }
            else
            {
                apnsMsg = apnsMsg.Replace("##SOUND##", "default");
                apnsMsg = apnsMsg.Replace("##SILENT##", "0");
            }

            //if (ParameterCache.IsProduction)
            {
                apnsMsg = apnsMsg.Replace("APNS_SANDBOX", "APNS");
            }

            return apnsMsg;
        }
    }
}
