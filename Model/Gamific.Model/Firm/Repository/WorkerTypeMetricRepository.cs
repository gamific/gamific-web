using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;
using Vlast.Gamific.Model.Firm.DTO;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class WorkerTypeMetricRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile WorkerTypeMetricRepository instance;

        private WorkerTypeMetricRepository() { }

        public static WorkerTypeMetricRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new WorkerTypeMetricRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Queries

        /// <summary>
        /// Busca todas associações ligadas a metrica 
        /// </summary>
        /// <returns></returns>
        public List<WorkerTypeMetricDTO> GetAllDTOFromMetric(string metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wtm in context.WorkerTypeMetrics
                            from wt in context.WorkerTypes
                            from m in context.Metrics
                            where wtm.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && m.Status == GenericStatus.ACTIVE
                            && wtm.MetricExternalId == metricId
                            && wt.Id == wtm.WorkerTypeId
                            && m.ExternalID == metricId
                            orderby wtm.Id ascending
                            select new WorkerTypeMetricDTO
                            {
                                Id = wtm.Id,
                                WorkerTypeId = wtm.WorkerTypeId,
                                MetricId = wtm.MetricId,
                                WorkerTypeName = wt.TypeName,
                                MetricName = m.MetricName
                            };

                return query.ToList();
            }
        }


        /// <summary>
        /// Busca todas associações ligadas a metrica 
        /// </summary>
        /// <returns></returns>
        public List<WorkerTypeMetricDTO> GetAllDTOFromMetric(string metricId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from wtm in context.WorkerTypeMetrics
                            from wt in context.WorkerTypes
                            where wtm.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && wtm.MetricExternalId == metricId
                            && wt.Id == wtm.WorkerTypeId
                            orderby wt.TypeName ascending
                            select new WorkerTypeMetricDTO
                            {
                                Id = wtm.Id,
                                WorkerTypeId = wtm.WorkerTypeId,
                                MetricId = wtm.MetricId,
                                WorkerTypeName = wt.TypeName,
                            }).OrderBy(x => x.WorkerTypeName).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }


        /// <summary>
        /// Conta todas associações ligadas a metrica 
        /// </summary>
        /// <returns></returns>
        public int GetCountFromMetric(string metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                int count = (from wtm in context.WorkerTypeMetrics
                             where wtm.Status == GenericStatus.ACTIVE
                             && wtm.MetricExternalId == metricId
                             select wtm).Count();

                return count;
            }
        }

        /// <summary>
        /// Conta todas associações ligadas a metrica 
        /// </summary>
        /// <returns></returns>
        public int GetCountToAssociateFromMetric(string metricId, string firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var exceptionList = from wt in context.WorkerTypes
                                    from wtm in context.WorkerTypeMetrics
                                    where wt.Status == GenericStatus.ACTIVE
                                    && wtm.Status == GenericStatus.ACTIVE
                                    && wtm.MetricExternalId == metricId
                                    && wt.Id == wtm.WorkerTypeId
                                    && wt.ProfileName == Profiles.JOGADOR
                                    select wt.Id;

                int count = (from wt in context.WorkerTypes
                            where wt.Status == GenericStatus.ACTIVE
                            && wt.ExternalFirmId == firmId
                            && !exceptionList.Contains(wt.Id)
                            && (wt.ProfileName == Profiles.JOGADOR || wt.ProfileName == Profiles.LIDER)
                            select wt).Count();

                return count;
            }
        }

        /// <summary>
        /// Busca todas associações ligadas a metrica 
        /// </summary>
        /// <returns></returns>
        public List<WorkerTypeMetricEntity> GetAllFromMetric(int metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wtm in context.WorkerTypeMetrics
                            from wt in context.WorkerTypes
                            from m in context.Metrics
                            where wtm.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && m.Status == GenericStatus.ACTIVE
                            && wtm.MetricId == metricId
                            && wt.Id == wtm.WorkerTypeId
                            && m.Id == metricId
                            orderby wtm.Id ascending
                            select wtm;

                return query.ToList();
            }
        }


        /// <summary>
        /// Busca todas associações ligadas ao tipo do jogador
        /// </summary>
        /// <param name="WorkerTypeId"></param>
        /// <returns></returns>
        public List<WorkerTypeMetricDTO> GetAllFromWorkerType(int WorkerTypeId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wtm in context.WorkerTypeMetrics
                            from wt in context.WorkerTypes
                            where wtm.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && wtm.WorkerTypeId == WorkerTypeId
                            && wt.Id == WorkerTypeId
                            orderby wtm.Id ascending
                            select new WorkerTypeMetricDTO
                            {
                                Id = wtm.Id,
                                WorkerTypeId = wtm.WorkerTypeId,
                                MetricExternalId = wtm.MetricExternalId,
                                WorkerTypeName = wt.TypeName
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todas associações ligadas ao tipo do jogador
        /// </summary>
        /// <param name="WorkerTypeId"></param>
        /// <returns></returns>
        public List<WorkerTypeMetricDTO> GetAllFromWorkerByPlayerId(string playerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from wtm in context.WorkerTypeMetrics
                            from wt in context.WorkerTypes
                            where wtm.Status == GenericStatus.ACTIVE
                            && wt.Status == GenericStatus.ACTIVE
                            && worker.ExternalId == playerId
                            && wtm.WorkerTypeId == worker.WorkerTypeId
                            && wt.Id == worker.WorkerTypeId
                            orderby wtm.Id ascending
                            select new WorkerTypeMetricDTO
                            {
                                Id = wtm.Id,
                                WorkerTypeId = wtm.WorkerTypeId,
                                MetricExternalId = wtm.MetricExternalId,
                                WorkerTypeName = wt.TypeName
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todas associações ligadas ao tipo do jogador
        /// </summary>
        /// <param name="WorkerTypeId"></param>
        /// <returns></returns>
        public List<WorkerTypeEntity> GetAllToAssociateFromFirm(string metricId, string firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var exceptionList = from wt in context.WorkerTypes
                                    from wtm in context.WorkerTypeMetrics
                                    where wt.Status == GenericStatus.ACTIVE
                                    && wtm.Status == GenericStatus.ACTIVE
                                    && wtm.MetricExternalId == metricId
                                    && wt.Id == wtm.WorkerTypeId
                                    && wt.ProfileName == Profiles.JOGADOR
                                    && wt.ExternalFirmId == firmId
                                    select wt.Id;

                var query = from wt in context.WorkerTypes
                            where wt.Status == GenericStatus.ACTIVE
                            && wt.ExternalFirmId == firmId
                            && !exceptionList.Contains(wt.Id)
                            && (wt.ProfileName == Profiles.JOGADOR || wt.ProfileName == Profiles.LIDER)
                            select wt;


                return query.ToList();
            }
        }

        /// <summary>
        /// Busca todos os tipos de jogadores ainda nao associadas a metrica 
        /// </summary>
        /// <param name="WorkerTypeId"></param>
        /// <returns></returns>
        public List<WorkerTypeEntity> GetAllToAssociate(string metricId, string firmId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var exceptionList = from wt in context.WorkerTypes
                                    from wtm in context.WorkerTypeMetrics
                                    where wt.Status == GenericStatus.ACTIVE
                                    && wtm.Status == GenericStatus.ACTIVE
                                    && wtm.MetricExternalId == metricId
                                    && wt.Id == wtm.WorkerTypeId
                                    select wt.Id;

                var query = (from wt in context.WorkerTypes
                             where wt.Status == GenericStatus.ACTIVE
                             && wt.ExternalFirmId == firmId
                             && !exceptionList.Contains(wt.Id)
                             && (wt.ProfileName == Profiles.JOGADOR || wt.ProfileName == Profiles.LIDER)
                             select wt).OrderBy(x => x.TypeName).Skip(pageIndex * pageSize).Take(pageSize);


                return query.ToList();
            }
        }

        /// <summary>
        /// Recupera uma associação de tipo de jogador e metrica pelo id
        /// </summary>
        /// <param name="workerTypeMetricId"></param>
        /// <returns></returns>
        public WorkerTypeMetricEntity GetById(int workerTypeMetricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wtm in context.WorkerTypeMetrics
                            where wtm.Id == workerTypeMetricId
                            && wtm.Status == GenericStatus.ACTIVE
                            select wtm;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Recupera todas as metricas de um dado tipo de jogador
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public List<MetricEntity> GetMetricsFromWorkerType(int workerTypeId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wtm in context.WorkerTypeMetrics
                            from m in context.Metrics
                            where wtm.Status == GenericStatus.ACTIVE
                            && wtm.MetricId == m.Id
                            && wtm.WorkerTypeId == workerTypeId
                            select m;

                return query.ToList();
            }

        }

        /// <summary>
        /// Salva uma nova associação entre tipo de funcionario e metrica na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public WorkerTypeMetricEntity CreateWorkerTypeMetric(WorkerTypeMetricEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.MetricId = 9;
                newEntity.LastUpdate = DateTime.UtcNow;
                context.WorkerTypeMetrics.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }

            return newEntity;
        }

        /// <summary>
        /// Atualiza a associação entre um tipo de funcionario e uma metrica
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public WorkerTypeMetricEntity UpdateWorkerTypeMetric(WorkerTypeMetricEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.WorkerTypeMetrics.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
