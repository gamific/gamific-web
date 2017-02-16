using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Properties;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class MetricRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile MetricRepository instance;

        private MetricRepository() { }

        public static MetricRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new MetricRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region CampaignMetric

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<MetricEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Metrics
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Busca todas ativas de uma empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MetricEntity> GetAllFromFirm(int firmId, int pageIndex = 0, int pageSize = 10)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from metric in context.Metrics
                            where metric.FirmId == firmId
                            && metric.Status == GenericStatus.ACTIVE
                            select metric).OrderBy(x => x.MetricName).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca pelo id
        /// </summary>
        /// <param name="metricId"></param>
        /// <returns></returns>
        public MetricEntity GetById(int metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Metrics
                            where t.Id == metricId && t.Status == GenericStatus.ACTIVE
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca tipo de metricas pelo id do time
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public List<MetricEntity> GetMetricsTeamById(int teamId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wtm in context.WorkerTypeMetrics
                            from metric in context.Metrics
                            from team in context.Teams
                            where metric.Status == GenericStatus.ACTIVE
                            && wtm.Status == GenericStatus.ACTIVE
                            && team.Id == teamId
                            && wtm.WorkerTypeId == team.WorkerTypeId
                            && wtm.MetricId == metric.Id
                            select metric;

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca tipo de metricas pelo id do funcionario
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public List<WorkerTypeMetricDTO> GetMetricsWorkerById(int workerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from wtm in context.WorkerTypeMetrics
                            from metric in context.Metrics
                            from worker in context.Workers
                            where metric.Status == GenericStatus.ACTIVE
                            && wtm.Status == GenericStatus.ACTIVE
                            && worker.Id == workerId
                            && wtm.WorkerTypeId == worker.WorkerTypeId
                            && wtm.MetricId == metric.Id
                            select new WorkerTypeMetricDTO
                            {
                                MetricId = metric.Id,
                                MetricName = metric.MetricName,
                                WorkerTypeId = wtm.Id,
                                Id = wtm.Id
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca a quantidade de metricas de uma firma
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public int GetCountFromFirm(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var count = (from metric in context.Metrics
                             where metric.Status == GenericStatus.ACTIVE
                             && metric.FirmId == firmId
                             select metric).Count();

                return count;
            }
        }


        /// <summary>
        /// cria
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public MetricEntity CreateMetric(MetricEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Metrics.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// atualiza
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public MetricEntity UpdateMetric(MetricEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Metrics.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
