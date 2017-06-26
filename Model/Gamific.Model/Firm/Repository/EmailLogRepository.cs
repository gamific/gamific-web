using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class EmailLogRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile EmailLogRepository instance;

        private EmailLogRepository() { }

        public static EmailLogRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new EmailLogRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        /// <summary>
        /// Criar Log de um email
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public EmailLogEntity Create(EmailLogEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.SendTime = DateTime.UtcNow;
                context.EmailLogs.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }

            return newEntity;
        }
    }
}
