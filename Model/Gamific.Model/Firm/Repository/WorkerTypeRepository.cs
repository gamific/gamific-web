using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class WorkerTypeRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile WorkerTypeRepository instance;

        private WorkerTypeRepository() { }

        public static WorkerTypeRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new WorkerTypeRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Profile

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<WorkerTypeEntity> GetAll(int firmId)
        {
            ModelContext context = new ModelContext();
            var query = from wt in context.WorkerTypes
                        where wt.Status == GenericStatus.ACTIVE
                        && wt.ProfileName != DTO.Profiles.ADMINISTRADOR
                        && wt.FirmId == firmId
                        orderby wt.Id ascending
                        select wt;

            return query.ToList();
        }

        /// <summary>
        /// Busca todos da empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<WorkerTypeEntity> GetAllFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.WorkerTypes
                            where sc.Status == GenericStatus.ACTIVE
                            && sc.FirmId == firmId
                            orderby sc.Id ascending
                            select sc;

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos da empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<WorkerTypeEntity> GetAllByGameId(string gameId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.WorkerTypes
                            where sc.Status == GenericStatus.ACTIVE
                            && sc.ExternalFirmId == gameId
                            orderby sc.Id ascending
                            select sc;

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos da empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public WorkerTypeEntity GetByGameIdAndTypeName(string gameId, string typeName)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wt in context.WorkerTypes
                            where wt.Status == GenericStatus.ACTIVE
                            && wt.ExternalFirmId == gameId
                            && wt.TypeName == typeName
                            select wt;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca todos da empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<WorkerTypeEntity> GetAllFromFirm(string firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from sc in context.WorkerTypes
                            where sc.Status == GenericStatus.ACTIVE
                            && sc.ExternalFirmId == firmId
                            orderby sc.Id ascending
                            select sc;

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos da empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<WorkerTypeEntity> GetAllFromFirm(string firmId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from wt in context.WorkerTypes
                            where wt.Status == GenericStatus.ACTIVE
                            && wt.ExternalFirmId == firmId
                            select wt).OrderBy(x => x.TypeName).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        /// <summary>
        /// Retorna a quantidade de tipos de jogadores da empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public int GetCountFromFirm(string firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                int count = (from wt in context.WorkerTypes
                             where wt.Status == GenericStatus.ACTIVE
                             && wt.ExternalFirmId == firmId
                             orderby wt.TypeName ascending
                             select wt).Count();

                return count;
            }
        }

        /// <summary>
        /// Recupera o tipo de funcionario pelo id
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public WorkerTypeEntity GetById(int workerTypeId)
        {
            try
            {
                using (ModelContext context = new ModelContext())
                {
                    var query =
                                from t in context.WorkerTypes
                                where t.Status == GenericStatus.ACTIVE && t.Id == workerTypeId
                                select t;

                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }

            return null;
        }

        /// <summary>
        /// Salva um tipo de funcionario na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public WorkerTypeEntity CreateWorkerType(WorkerTypeEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.WorkerTypes.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza um tipo de funcionario
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public WorkerTypeEntity UpdateWorkerType(WorkerTypeEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.WorkerTypes.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
