using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Public.Domain;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Public.Repository
{
    public class TopicHelpRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile TopicHelpRepository instance;

        private TopicHelpRepository() { }

        public static TopicHelpRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new TopicHelpRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region TopicHelp

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<TopicHelpEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.TopicHelps
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Busco todos de uma determinada empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<TopicHelpEntity> GetAllFromFirm(int firmId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.TopicHelps
                        where sc.Status == GenericStatus.ACTIVE && sc.FirmId == firmId
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Recupera o topico da ajuda pelo id
        /// </summary>
        /// <param name="topicHelpId"></param>
        /// <returns></returns>
        public TopicHelpEntity GetById(int topicHelpId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.TopicHelps
                            where t.Status == GenericStatus.ACTIVE && t.Id == topicHelpId
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva um topico de ajuda na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public TopicHelpEntity CreateTopicHelp(TopicHelpEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.TopicHelps.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza um topico de ajuda
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public TopicHelpEntity UpdateTopicHelp(TopicHelpEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.TopicHelps.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
