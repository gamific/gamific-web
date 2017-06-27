using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class DataRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile DataRepository instance;

        private DataRepository() { }

        public static DataRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new DataRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Data

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DataEntity> GetAll(int pageIndex = 10, int pageSize = 10)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Datas
                        orderby sc.Id ascending
                        select sc;

            return query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DataEntity> GetAllOfGameIdAndGameName()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Datas
                        orderby sc.Id ascending
                        select sc;

            return query.OrderBy(x => x.FirmName).ToList();
        }

        /// <summary>
        /// Recupera a empresa pelo id
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public DataEntity GetById(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Datas
                            where t.Id == firmId
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Recupera a empresa pelo id
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public DataEntity GetByExternalId(string externalId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from firm in context.Datas
                            where firm.ExternalId == externalId
                            select firm;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva uma empresa na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public DataEntity CreateFirm(DataEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Datas.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza uma empresa
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public DataEntity UpdateFirm(DataEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Datas.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        /// <summary>
        /// Recupera a empresa de um colaborador
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public DataEntity GetWorkerFirm(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.Datas
                            from t in context.Workers
                            where t.UserId == workerId && sc.Id == t.FirmId && sc.Status == GenericStatus.ACTIVE && t.Status == GenericStatus.ACTIVE
                            select sc;

                return query.FirstOrDefault();
            }
        }

        #endregion
    }
}
