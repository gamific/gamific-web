using Amazon.SimpleNotificationService.Model;
using Vlast.Broker.Platform.Microsoft;

using Vlast.Util.Aws;
using Vlast.Util.Instrumentation;
using System;
using Vlast.Broker.SNS.Model;

namespace Vlast.Broker.Client
{
    /// <summary>
    /// Configuração de notificação para Windows notification service usando o AWS SNS
    /// </summary>
    public abstract class SNSDispatcher
    {

        /// <summary>
        /// Envia uma notificação push para todos os dispositivos
        /// que assinam o tópico 
        /// </summary>
        /// <param name="topicARn"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        ///  <param name="silent"></param>
        /// <param name="extras"></param>
        /// <returns></returns>
        public static string SendPushForSubscribers(string topicARn, string title, string message, string extras = "", bool silent = false)
        {
            title = SNSDispatcher.Scape(title);
            message = SNSDispatcher.Scape(message);

            string extraValue = extras;

            string wnsMessage = WNSDispatcher.Instance.GetMessage(title, message, silent, extraValue);
            string apnsMessage = APNSDispatcher.Instance.GetMessage(title, message, silent, extraValue);
            string gcmMessage = GCMDispatcher.Instance.GetMessage(title, message, silent, extraValue);

            string topicMsg = "{ \"default\" : \"Mensagem Recebida. \"," + wnsMessage + ", " + apnsMessage + ", " + gcmMessage + "}";

            PublishRequest request = new PublishRequest()
            {
                Message = topicMsg,
                MessageStructure = "json",
                TopicArn = topicARn,
                MessageAttributes = WNSDispatcher.Instance.GetWNSHeaders(silent)
            };

            var result = AWSFactory.SNSClient.Publish(request);

            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.MessageId;
            }

            return null;
        }

        /// <summary>
        /// Envia uma notificação para a plataforma do device
        /// </summary>
        /// <param name="device"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public static bool SendDevicePush(DeviceEntity device, string title, string message, bool silent, string extra = "")
        {
            try
            {
                switch (device.DeviceType)
                {
                    case DeviceType.ANDROID:
                        return GCMDispatcher.Instance.SendMessage(device, title, message, silent, extra);
                    case DeviceType.IOS:
                        return APNSDispatcher.Instance.SendMessage(device, title, message, silent, extra);
                    case DeviceType.WINDOWS_PHONE:
                        return WNSDispatcher.Instance.SendMessage(device, title, message, silent, extra);
                    default: return false;
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// Envia um notification push para o dispositivo
        /// </summary>
        /// <param name="device"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        protected abstract bool SendMessage(DeviceEntity device, string title, string message, bool silent, string extra = "");

        /// <summary>
        /// Formata a mensagem na plataforma específica
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public abstract string GetMessage(string title, string message, bool silent = false, string extra = "");

        /// <summary>
        /// Remove caracteres especiais
        /// </summary>
        /// <returns></returns>
        internal static string Scape(string data)
        {
            data = data.Replace("<", " ");
            data = data.Replace(">", " ");
            data = data.Replace("<", " ");
            data = data.Replace("'", " ");
            data = data.Replace("&", " ");
            data = data.Replace("\n", " ");
            data = data.Replace("\r", " ");
            data = data.Replace("\"", " ");
            data = data.Replace("\t", " ");

            return data;
        }
    }
}
