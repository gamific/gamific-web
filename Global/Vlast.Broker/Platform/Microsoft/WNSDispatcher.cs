using Amazon.SimpleNotificationService.Model;

using Vlast.Util.Aws;
using Vlast.Util.Parameter;
using System;
using System.Collections.Generic;
using Vlast.Broker.Properties;
using Vlast.Broker.Client;
using Vlast.Broker.SNS.Model;

namespace Vlast.Broker.Platform.Microsoft
{
    /// <summary>
    /// Configuração de notificação para Windows notification service usando o AWS SNS
    /// </summary>
    public class WNSDispatcher : SNSDispatcher
    {

        #region Configuração

        private static volatile WNSDispatcher instance;
        private static object syncRoot = new Object();

        private WNSDispatcher()
        {
        }

        public static WNSDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new WNSDispatcher();
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
                return ParameterCache.Get("SNS_APP_WP_NAME");
            }
        }

        #endregion

        /// <summary>
        /// Envia um notification push para um device usando o sistema de notificações da Microsoft
        /// </summary>
        /// <param name="device">ARN registrado no amazon</param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        protected override bool SendMessage(DeviceEntity device, string title, string message, bool silent, string extra = "")
        {
            string toast = "{ " + GetMessage(title, message, silent, extra) + " } ";

            PublishRequest snsRequest = new PublishRequest()
            {
                Message = toast,
                MessageStructure = "json",
                MessageAttributes = GetWNSHeaders(silent),
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
        /// Formata a mensagem para WNS
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public override string GetMessage(string title, string message, bool silent, string extra = "")
        {
            message = (message.Length > 140 ? message.Substring(0, 140) : message);
            message = !string.IsNullOrWhiteSpace(message) ? message + "..." : message;

            string wnsMsg = "";
            if (!silent)
            {
                wnsMsg = string.Format(Resources.WPNS_TOAST, title, message, extra);
            }
            else
            {
                wnsMsg = extra;
            }


            return wnsMsg;
        }

        /// <summary>
        /// Constrou os Headers de notificação
        /// </summary>
        /// <param name="silent"></param>
        /// <returns></returns>
        internal Dictionary<string, Amazon.SimpleNotificationService.Model.MessageAttributeValue> GetWNSHeaders(bool silent)
        {
            Dictionary<string, Amazon.SimpleNotificationService.Model.MessageAttributeValue> sendHeaders = new Dictionary<string, Amazon.SimpleNotificationService.Model.MessageAttributeValue>();
            sendHeaders.Add("AWS.SNS.MOBILE.WNS.CachePolicy", new Amazon.SimpleNotificationService.Model.MessageAttributeValue() { DataType = "String", StringValue = "cache" });

            if (!silent)
            {
                sendHeaders.Add("AWS.SNS.MOBILE.WNS.Type", new Amazon.SimpleNotificationService.Model.MessageAttributeValue() { DataType = "String", StringValue = "wns/toast" });
            }
            else
            {
                sendHeaders.Add("AWS.SNS.MOBILE.WNS.Type", new Amazon.SimpleNotificationService.Model.MessageAttributeValue() { DataType = "String", StringValue = "wns/raw" });
            }

            return sendHeaders;
        }
    }
}
