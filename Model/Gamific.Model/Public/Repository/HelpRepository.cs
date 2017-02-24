using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Public.Domain;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Public.Repository
{
    public class HelpRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile HelpRepository instance;

        private HelpRepository() { }

        public static HelpRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new HelpRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Help

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<HelpEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Helps
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
        public List<HelpEntity> GetAllFromFirm(int firmId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Helps
                        where sc.Status == GenericStatus.ACTIVE //&& sc.FirmId == firmId
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Busca todos de um determinado topico
        /// </summary>
        /// <param name="topicHelpId"></param>
        /// <returns></returns>
        public List<HelpEntity> GetAllFromTopic(int topicHelpId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Helps
                        where sc.Status == GenericStatus.ACTIVE && sc.TopicId == topicHelpId
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Recupera a ajuda pelo id
        /// </summary>
        /// <param name="helpId"></param>
        /// <returns></returns>
        public HelpEntity GetById(int helpId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Helps
                            where t.Status == GenericStatus.ACTIVE && t.Id == helpId
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva uma ajuda na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public HelpEntity CreateHelp(HelpEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Helps.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza uma ajuda
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public HelpEntity UpdateHelp(HelpEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Helps.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
