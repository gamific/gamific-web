using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Vlast.Broker.Platform.Microsoft;

using Vlast.Util.Aws;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vlast.Util.Data;
using System.Transactions;
using Vlast.Util.Instrumentation;
using Vlast.Broker.SNS.Model;

namespace Vlast.Broker.SNS
{
    /// <summary>
    /// Trata o registro dos dispositivos no AWS SNS
    /// </summary>
    public class DeviceHandler
    {
       
        /// <summary>
        /// Registra um dispositivo no Aws SNS e retorna o ARN para posteriores notificações
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static string RegisterNotificationEndpoint(DeviceEntity device)
        {

            string platFormArn = "";

            switch (device.DeviceType)
            {
                case DeviceType.WINDOWS_PHONE:
                    platFormArn = WNSDispatcher.SNS_APP_ARN;
                    break;
                case DeviceType.IOS:
                    platFormArn = APNSDispatcher.SNS_APP_ARN;
                    break;
                case DeviceType.ANDROID:
                    platFormArn = GCMDispatcher.SNS_APP_ARN;
                    break;
                default:
                    platFormArn = "";
                    break;
            }

            // Find in aws exception 
            string existingEndpointRegexString = "Reason: Endpoint (.+) already exists with the same Token";
            Regex existingEndpointRegex = new Regex(existingEndpointRegexString);

            CreatePlatformEndpointRequest request = new CreatePlatformEndpointRequest()
            {
                PlatformApplicationArn = platFormArn,
                Token = device.Endpoint,
                CustomUserData = device.UserId + ""
            };

            CreatePlatformEndpointResponse result = null;
            try
            {
                result = AWSFactory.SNSClient.CreatePlatformEndpoint(request);
            }
            catch (AmazonSimpleNotificationServiceException e)
            {
                if (e.ErrorCode == "InvalidParameter")
                {
                    var match = existingEndpointRegex.Match(e.Message);
                    if (match.Success)
                    {
                        string arn = match.Groups[1].Value;
                        AWSFactory.SNSClient.DeleteEndpoint(new DeleteEndpointRequest
                        {
                            EndpointArn = arn,
                        });

                        result = AWSFactory.SNSClient.CreatePlatformEndpoint(request);
                    }
                }
            }

            if (result != null && result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.EndpointArn;
            }
            return null;
        }

        /// <summary>
        /// Atualiza o registro de endpoint de notificação no SNS
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static string UpdateNotificationEndpoint(DeviceEntity device)
        {
            SetEndpointAttributesRequest request = new SetEndpointAttributesRequest()
            {
                EndpointArn = device.BrokerEndpoint,
            };

            Dictionary<string, string> atributes = new Dictionary<string, string>();
            atributes.Add("Token", device.Endpoint);
            atributes.Add("Enabled", "true");
            request.Attributes = atributes;

            var result = AWSFactory.SNSClient.SetEndpointAttributes(request);
            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return device.BrokerEndpoint;
            }
            return null;
        }

        /// <summary>
        /// Atualiza o registro de endpoint de notificação no SNS
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool RemoveNotificationEndpoint(DeviceEntity device)
        {
            DeleteEndpointRequest request = new DeleteEndpointRequest()
            {
                EndpointArn = device.BrokerEndpoint
            };

            var result = AWSFactory.SNSClient.DeleteEndpoint(request);
            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Registra um novo device para o usuário
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static DeviceEntity RegisterNewUserDevice(long currentUserId, DeviceEntity device )
        {
            DeviceEntity result = null;
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                PushRepository.Instance.UnsubscribeAll(currentUserId);
                DeviceRepository.Instance.RemoveAllDevices(currentUserId);

                //Cria o novo device
                DeviceEntity newDevice = new DeviceEntity()
                {
                    UserId = currentUserId,
                    Status = GenericStatus.ACTIVE,
                    Endpoint = device.Endpoint,
                    DeviceType = device.DeviceType
                };

                string snsEndpoint = DeviceHandler.RegisterNotificationEndpoint(newDevice);
                if (string.IsNullOrEmpty(snsEndpoint))
                {
                    ServiceHelper.ThrowError("Unable to register Device " + device.Id + " .");
                }

                newDevice.BrokerEndpoint = snsEndpoint;
                string subscriptionResult = TopicHandler.SubscribeTopic(TopicHandler.SNS_DEFAULT_TOPIC, newDevice);
                if (string.IsNullOrEmpty(subscriptionResult))
                {
                    ServiceHelper.ThrowError("Unable to register Device " + device.Id + " .");
                }
                

                result = DeviceRepository.Instance.CreateDevice(newDevice);

                tran.Complete();
            }
            return result;
        }

        /// <summary>
        /// Atualiza um novo device para o usuário
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static DeviceEntity UpdateNewUserDevice(long currentUserId, DeviceEntity current, string endpoint)
        {
            DeviceEntity result = null;
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                current.Endpoint = endpoint;

                DeviceHandler.UpdateNotificationEndpoint(current);

                result = DeviceRepository.Instance.UpdateDevice(current);

                tran.Complete();
            }
            return result;
        }

    }
}
