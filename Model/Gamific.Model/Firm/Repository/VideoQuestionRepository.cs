using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class VideoQuestionRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile VideoQuestionRepository instance;

        private VideoQuestionRepository() { }

        public static VideoQuestionRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new VideoQuestionRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region VideoQuestion

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<VideoQuestionEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.VideoQuestions
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<VideoQuestionEntity> GetAllByVideo(int videoId, int firmId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.VideoQuestions
                        where sc.VideoId == videoId && sc.FirmId == firmId && sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Recupera a mensagem pelo id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public VideoQuestionEntity GetById(int videoQuestionId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.VideoQuestions
                            where t.Id == videoQuestionId && t.Status == GenericStatus.ACTIVE
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva uma mensagem na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public VideoQuestionEntity CreateVideoQuestion(VideoQuestionEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.VideoQuestions.Attach(newEntity);
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
        public VideoQuestionEntity UpdateVideoQuestion(VideoQuestionEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.VideoQuestions.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion

    }
}