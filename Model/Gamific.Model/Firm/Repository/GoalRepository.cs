using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;
using Vlast.Gamific.Model.Firm.DTO;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class GoalRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile GoalRepository instance;

        private GoalRepository() { }

        public static GoalRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new GoalRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Queries

        public List<GoalDTO> GetAllFromWorkerByRunId(string runId, List<MetricEntity> metrics)
        {
            using (ModelContext context = new ModelContext())
            {
                var goals = from metric in metrics
                            join goal in (from goal in context.Goals where goal.RunId == runId select goal)
                            on metric.ExternalID equals goal.ExternalMetricId into mg
                            from subGoal in mg.DefaultIfEmpty()
                            select new GoalDTO
                            {
                                EpisodeId = (subGoal == null ? null : subGoal.EpisodeId),
                                MetricName = metric.MetricName,
                                Icon = metric.Icon,
                                ExternalMetricId = metric.ExternalID,
                                RunId = runId,
                                Goal = (subGoal == null ? 0 : subGoal.Goal),
                                GoalId = (subGoal == null ? 0 : subGoal.Id)
                            };

                return goals.ToList();
            }
        }

        public List<GoalDTO> GetByRunId(string runId)
        {
            using (ModelContext context = new ModelContext())
            {
                var goals = from goal in context.Goals
                            where goal.RunId == runId
                            select new GoalDTO
                            {
                                ExternalMetricId = goal.ExternalMetricId,
                                RunId = goal.RunId,
                                Goal = goal.Goal,
                                GoalId = goal.Id
                            };

                return goals.ToList();
            }
        }

        public List<GoalDTO> GetByRunId(List<string> runIds)
        {
            using (ModelContext context = new ModelContext())
            {
                var goals = (from goal in context.Goals
                             from runId in runIds
                             where goal.RunId == runId
                             group goal by goal.ExternalMetricId into g
                             select new GoalDTO
                             {
                                 ExternalMetricId = g.FirstOrDefault().ExternalMetricId,
                                 EpisodeId = g.FirstOrDefault().EpisodeId,
                                 RunId = g.FirstOrDefault().RunId,
                                 Goal = g.Sum(s => s.Goal),
                                 GoalId = g.FirstOrDefault().Id
                             });

                return goals.ToList();
            }
        }

        public GoalDTO GetByRunIdAndMetricId(string runId, string metricId)
        {
            using (ModelContext context = new ModelContext())
            {
                var goals = from goal in context.Goals
                            where goal.RunId == runId
                            && goal.ExternalMetricId == metricId
                            select new GoalDTO
                            {
                                ExternalMetricId = goal.ExternalMetricId,
                                RunId = goal.RunId,
                                Goal = goal.Goal,
                                GoalId = goal.Id
                            };

                return goals.FirstOrDefault();
            }
        }

        public List<GoalDTO> GetByEpisodeId(string episodeId)
        {
            using (ModelContext context = new ModelContext())
            {
                var goals = (from goal in context.Goals
                             where goal.EpisodeId == episodeId
                             group goal by goal.ExternalMetricId into g
                             select new GoalDTO
                             {
                                 ExternalMetricId = g.FirstOrDefault().ExternalMetricId,
                                 EpisodeId = g.FirstOrDefault().EpisodeId,
                                 RunId = g.FirstOrDefault().RunId,
                                 Goal = g.Sum(s => s.Goal),
                                 GoalId = g.FirstOrDefault().Id
                             });

                return goals.ToList();
            }
        }

        /// <summary>
        /// Recupera a meta total em pontos de um tipo de jogador
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        public int GetTotalGoalFromWorkerType(int workerTypeId)
        {
            using (ModelContext context = new ModelContext())
            {
                int totalGoal = (from metric in context.Metrics
                                 from wtm in context.WorkerTypeMetrics
                                 where metric.Status == GenericStatus.ACTIVE
                                 && wtm.Status == GenericStatus.ACTIVE
                                 && wtm.WorkerTypeId == workerTypeId
                                 && wtm.MetricId == metric.Id
                                 select metric).Select(s => s.Weigth).Sum();

                return totalGoal; 
            }
        }

        /// <summary>
        /// Salva uma nova meta
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public GoalEntity CreateGoal(GoalEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Goals.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }

            return newEntity;
        }

        /// <summary>
        /// Atualiza uma meta
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public GoalEntity UpdateGoal(GoalEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Goals.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}
