using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Broker.SNS;
using Vlast.Util.Data;

namespace Vlast.Broker.SNS.Model
{
    public class PushRepository
    {

        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile PushRepository instance;

        private PushRepository() { }

        public static PushRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new PushRepository();
                    }
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// Remove todas as assinaturas de um usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UnsubscribeAll(long userId)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                string query = "UPDATE  Push_Subscription SET Status = 0 WHERE  UserId = " + userId;
                int count = context.Database.ExecuteSqlCommand(query);
                return count > 0;
            }
        }

        /// <summary>
        /// Remove todas as assinaturas de um usuário com o filtro de topico
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UnsubscribeAll(long userId, string topicPrefix)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                string query = "UPDATE  Push_Subscription SET Status = 0 WHERE  UserId = " + userId + " AND Topic LIKE '" + topicPrefix + "%'" ;
                int count = context.Database.ExecuteSqlCommand(query);
                return count > 0;
            }
        }

        /// <summary>
        /// Recupera a assinatura de um tópico
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<PushSubscriptionEntity> GetPushSubscriptions(long userId, string topic)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                var query = from p in context.PushSubscriptions
                            where p.Status == GenericStatus.ACTIVE
                                && p.UserId == userId && p.Topic.Contains(topic)
                            select p;

                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera a assinatura de um tópico
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public PushSubscriptionEntity GetPushSubscription(long userId, long deviceId, string topic)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                var query = from p in context.PushSubscriptions
                            where p.Status == GenericStatus.ACTIVE
                                && p.UserId == userId
                                && p.DeviceId == deviceId
                                && p.Topic == topic
                            select p;

                return query.FirstOrDefault();
            }
        }


        /// <summary>
        /// Cria uma assinatura para a categoria
        /// </summary>
        /// <param name="subscription"></param>
        public void RegisterSubscription(PushSubscriptionEntity subscription)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                subscription.Status = GenericStatus.ACTIVE;
                subscription.LastUpdate = DateTime.UtcNow;
                context.PushSubscriptions.Attach(subscription);
                context.Entry(subscription).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Atualiza uma assinatura para a categoria
        /// </summary>
        /// <param name="subscription"></param>
        public void UpdateSubscription(PushSubscriptionEntity subscription)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                subscription.LastUpdate = DateTime.UtcNow;
                context.PushSubscriptions.Attach(subscription);
                context.Entry(subscription).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
