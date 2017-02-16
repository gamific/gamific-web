using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;
using Vlast.Gamific.Model.Public.DTO;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class ResultRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile ResultRepository instance;

        private ResultRepository() { }

        public static ResultRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new ResultRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Result

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<ResultEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Results
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Busca pelo Id
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public ResultEntity GetById(int resultId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Results
                            where t.Id == resultId
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Busca a soma de todos os resultados de um funcionario em uma faixa de tempo
        /// </summary>
        /// <param name="workerId"></param>
        /// <returns></returns>
        public List<ResultMetricDTO> GetWorkerResultsFromMetric(int workerId, int metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from result in context.Results
                            from profile in context.Profiles
                            from worker in context.Workers
                            where result.MetricId == metricId
                            && result.WorkerId == workerId
                            && worker.Id == workerId
                            && profile.Id == worker.UserId
                            select new ResultMetricDTO
                            {
                                ResultId = result.Id,
                                GoalId = 0,
                                MetricId = metricId,
                                Goal = 0,
                                Result = result.Result,
                                Points = 0,
                                MetricIcon = "",
                                MetricName = "",
                                Date = result.Period,
                                WorkerName = profile.Name
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// busca
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public List<ResultMetricDTO> GetTeamResultsFromMetric(int teamId, int metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from tw in context.TeamWorkers
                            from wt in context.WorkerTypes
                            from result in context.Results
                            where worker.Status == GenericStatus.ACTIVE
                            && tw.Status == GenericStatus.ACTIVE
                            && worker.WorkerTypeId == wt.Id
                            && wt.ProfileName == DTO.Profiles.JOGADOR
                            && tw.TeamId == teamId
                            && tw.WorkerId == worker.Id
                            && result.WorkerId == worker.Id
                            && result.MetricId == metricId
                            && profile.Id == worker.UserId
                            select new ResultMetricDTO
                            {
                                ResultId = result.Id,
                                GoalId = 0,
                                MetricId = metricId,
                                Goal = 0,
                                Result = result.Result,
                                Points = 0,
                                MetricIcon = "",
                                MetricName = "",
                                Date = result.Period,
                                WorkerName = profile.Name
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// busca
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public List<ResultMetricDTO> GetWorkerTypeResultsFromMetric(int workerTypeId, int metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from tw in context.TeamWorkers
                            from wt in context.WorkerTypes
                            from team in context.Teams
                            from result in context.Results
                            where worker.Status == GenericStatus.ACTIVE
                            && tw.Status == GenericStatus.ACTIVE
                            && worker.WorkerTypeId == wt.Id
                            && wt.ProfileName == DTO.Profiles.JOGADOR
                            && tw.TeamId == team.Id
                            && tw.WorkerId == worker.Id
                            && tw.WorkerId == worker.Id
                            && team.WorkerTypeId == workerTypeId
                            && result.WorkerId == worker.Id
                            && result.MetricId == metricId
                            && profile.Id == worker.UserId
                            select new ResultMetricDTO
                            {
                                ResultId = result.Id,
                                GoalId = 0,
                                MetricId = metricId,
                                Goal = 0,
                                Result = result.Result,
                                Points = 0,
                                MetricIcon = "",
                                MetricName = "",
                                Date = result.Period,
                                WorkerName = profile.Name
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// busca
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public List<ResultMetricDTO> GetFirmResultsFromMetric(int firmId, int metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from worker in context.Workers
                            from profile in context.Profiles
                            from result in context.Results
                            from wt in context.WorkerTypes
                            where worker.Status == GenericStatus.ACTIVE
                            && worker.WorkerTypeId == wt.Id
                            && wt.ProfileName == DTO.Profiles.JOGADOR
                            && result.FirmId == firmId
                            && result.WorkerId == worker.Id
                            && result.MetricId == metricId
                            && profile.Id == worker.UserId
                            select new ResultMetricDTO
                            {
                                ResultId = result.Id,
                                GoalId = 0,
                                MetricId = metricId,
                                Goal = 0,
                                Result = result.Result,
                                Points = 0,
                                MetricIcon = "",
                                MetricName = "",
                                Date = result.Period,
                                WorkerName = profile.Name
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Busca a soma de todos os resultados de uma firma em uma faixa de tempo
        /// </summary>
        /// <param name="mainResult"></param>
        /// <returns></returns>
        public List<ResultEntity> GetAllFromMainResult(int? mainResult)
        {
            if(mainResult != null)
            {
                using (ModelContext context = new ModelContext())
                {
                    var query = from result in context.Results
                                where result.MainResult == mainResult
                                select result;

                    return query.ToList();
                }
            }

            return null;
        }

        /// <summary>
        /// cria
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public ResultEntity CreateResult(ResultEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Results.Attach(newEntity);
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
        public ResultEntity UpdateResult(ResultEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Results.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }



        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public void RemoveEntity(int id)
        {
            using (ModelContext context = new ModelContext())
            {
                ResultEntity result = context.Results.Find(id);
                if (result != null)
                {
                    context.Results.Remove(result);
                    context.SaveChanges();
                }
            }
        }

        /**
        public bool SaveResultsFromArchive(List<ResultEntity> results)
        {

            using (ModelContext context = new ModelContext())
            {
                if (results != null && results.Count > 0)
                {
                    string bulkInsert = "INSERT INTO Firm_Result(FirmId, TeamWorkerId, CampaignTeamId, CampaignMetricId, Period, UpdatedBy, Result, LastUpdate) VALUES";
                    string rowTemplate = "({0},{1},{2},{3},'{4}',{5},{6}, '" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    int i = 0;

                    foreach (ResultEntity result in results)
                    {
                        string rowInsert = string.Format(rowTemplate, result.FirmId, result.TeamWorkerId, result.CampaignTeamId, result.CampaignMetricId, result.Period, result.UpdatedBy, result.Result);
                        if (i != results.Count() - 1)
                        {
                            rowInsert += ",";
                        }

                        bulkInsert += rowInsert;

                        i++;
                    }

                    if (bulkInsert.EndsWith(","))
                    {
                        bulkInsert = bulkInsert.Substring(0, bulkInsert.Length - 1);
                    }

                    bulkInsert += ";";

                    int count = context.Database.ExecuteSqlCommand(bulkInsert);

                    return count > 0;
                }

                return false;
            }
        }
    **/

        #endregion
    }
}
