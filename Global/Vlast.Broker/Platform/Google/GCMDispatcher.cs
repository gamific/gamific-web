using Amazon.SimpleNotificationService.Model;

using Vlast.Util.Aws;
using Vlast.Util.Instrumentation;
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
    public class GCMDispatcher : SNSDispatcher
    {
        #region Configuração

        private static volatile GCMDispatcher instance;
        private static object syncRoot = new Object();

        private GCMDispatcher()
        {
        }

        public static GCMDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GCMDispatcher();
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
                return ParameterCache.Get("SNS_APP_ANDROID_NAME");
            }
        }

        #endregion

        /// <summary>
        /// Envia um notification push para um device usando o sistema de notificações da Apple
        /// </summary>
        /// <param name="device">ARN registrado no amazon</param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        protected override bool SendMessage(DeviceEntity device, string title, string message, bool silent, string extra = "")
        {
            try
            {


                string msg = "{ " + GetMessage(title, message, silent, extra) + " }";

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
            catch (Exception ex)
            {
                ServiceHelper.ThrowError(ex);
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
        public override string GetMessage(string title, string message, bool silent = false, string extra = "")
        {
            message = (message.Length > 140 ? message.Substring(0, 140) : message);

            message = !string.IsNullOrWhiteSpace(message) ? message + "..." : message;

            string gcmMsg = Resources.GCM_MSG.Replace("##MSG##", message);
            gcmMsg = gcmMsg.Replace("##TITLE##", title);
            gcmMsg = gcmMsg.Replace("##EXTRA##", extra);

            return gcmMsg;
        }
    }
}
