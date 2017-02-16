using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Properties;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class VideoQuestionAnsweredRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile VideoQuestionAnsweredRepository instance;

        private VideoQuestionAnsweredRepository() { }

        public static VideoQuestionAnsweredRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new VideoQuestionAnsweredRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region VideoQuestionAnswered

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<VideoQuestionAnsweredEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.VideoQuestionAnswereds
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Recupera a mensagem pelo id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public VideoQuestionAnsweredEntity GetById(int videoQuestionAnsweredId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.VideoQuestionAnswereds
                            where t.Id == videoQuestionAnsweredId
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva uma mensagem na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public VideoQuestionAnsweredEntity CreateVideoQuestionAnswered(VideoQuestionAnsweredEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.AnsweredDate = DateTime.UtcNow;
                context.VideoQuestionAnswereds.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza uma mensagem
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public VideoQuestionAnsweredEntity UpdateVideoQuestionAnswered(VideoQuestionAnsweredEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.AnsweredDate = DateTime.UtcNow;
                context.VideoQuestionAnswereds.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion

    }
}
