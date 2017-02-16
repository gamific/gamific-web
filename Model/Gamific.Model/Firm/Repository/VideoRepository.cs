using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Properties;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class VideoRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile VideoRepository instance;

        private VideoRepository() { }

        public static VideoRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new VideoRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Video

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<VideoEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Videos
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Busca todos pela empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<VideoEntity> GetAllFromFirm(int firmId)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Videos
                        where sc.FirmId == firmId && sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Recupera o video pelo id
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public VideoEntity GetById(int videoId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Videos
                            where t.Id == videoId && t.Status == GenericStatus.ACTIVE
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva um video
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public VideoEntity CreateVideo(VideoEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Videos.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza um video
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public VideoEntity UpdateUpdate(VideoEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Videos.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion

    }
}
