using Amazon.SimpleNotificationService.Model;
using Vlast.Util.Aws;
using Vlast.Util.Parameter;
using System;
using Vlast.Broker.SNS.Model;

namespace Vlast.Broker.SNS
{
    /// <summary>
    /// Registro para notificação em bloco dentro dos canais
    /// </summary>
    public class TopicHandler
    {
        /// <summary>
        /// Retorna o endpoint de notificação para plataforma Microsoft
        /// </summary>
        public static string SNS_DEFAULT_TOPIC
        {
            get
            {
                return ParameterCache.Get("SNS_DEFAULT_TOPIC");
            }
        }
       

        /// <summary>
        /// Assina o topico SNS do canal para recebimento de push notification
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static string SubscribeTopic(string topicArn, DeviceEntity device)
        {
            return Subscribe(topicArn, device.BrokerEndpoint);
        }

        /// <summary>
        /// Abaga uma subscrição de tópico no sns
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static bool UnSubscribeTopic(string subscriptionArn)
        {
            if (string.IsNullOrWhiteSpace(subscriptionArn))
                return true;

            UnsubscribeRequest request = new UnsubscribeRequest()
            {
                SubscriptionArn = subscriptionArn
            };

            var subResult = AWSFactory.SNSClient.Unsubscribe(request);

            if (subResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Assina um tópico específico para um device específico
        /// </summary>
        /// <param name="topicEndPoint"></param>
        /// <param name="deviceEndpoint"></param>
        /// <returns></returns>
        private static string Subscribe(string topicEndPoint, string deviceEndpoint)
        {
            SubscribeRequest request = new SubscribeRequest()
            {
                Endpoint = deviceEndpoint,
                Protocol = "application",
                TopicArn = topicEndPoint
            };

            var subResult = AWSFactory.SNSClient.Subscribe(request);

            if (subResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return subResult.SubscriptionArn;
            }

            return null;
        }


        /// <summary>
        /// Cria um tópico específico para um nome passado como parâmetro
        /// </summary>
        /// <param name="topicName">Canal para criação do tópico</param>
        /// <returns>Arn do tópico criado</returns>
        public static string CreateTopic(string topicName)
        {
           
            CreateTopicRequest request = new CreateTopicRequest()
            {
                Name = topicName
            };

            var subResult = AWSFactory.SNSClient.CreateTopic(request);

            if (subResult.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return subResult.TopicArn;
            }

            return null;
        }

        public static void CreateTopic(object classTopic)
        {
            throw new NotImplementedException();
        }
    }
}
