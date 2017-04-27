using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using Vlast.Gamific.Web.Services.Push.DTO;

namespace Vlast.Gamific.Web.Services.Push
{
    public class NotificationPushService
    {
        private static string NotificationPushURL { get; } = WebConfigurationManager.AppSettings["NOTIFICATION_PUSH_URL"];

        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile NotificationPushService instance;

        private NotificationPushService() { }

        public static NotificationPushService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new NotificationPushService();
                    }
                }
                return instance;
            }
        }

        #endregion


        protected WebClient GetClient()
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            client.Headers[HttpRequestHeader.Accept] = "application/json";
            client.Encoding = System.Text.Encoding.UTF8;

            return client;
        }

        protected string JsonSerialize<T>(ref T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                                                new JsonSerializerSettings
                                                {
                                                    NullValueHandling = NullValueHandling.Ignore
                                                });
        }

        protected T JsonDeserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json,
                                                   new JsonSerializerSettings
                                                   {
                                                       NullValueHandling = NullValueHandling.Ignore,
                                                   });
        }

        public NotificationLogDTO SendPush(NotificationPushDTO notification)
        {
            try
            {
                using (WebClient client = GetClient())
                {

                    string response = client.DownloadString(NotificationPushURL + "sendpush" + "?token=" + notification.Token + "&msg=" + notification.Message + "&title=" + notification.Title + "&playerId=" + notification.PlayerId);
                    //return JsonDeserialize<NotificationLogDTO>(response);
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}