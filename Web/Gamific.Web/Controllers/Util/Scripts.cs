using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Util
{
    public class Scripts
    {
        public void MySqlGoalToEngineByEpisodeId(string episodeId)
        {
            List<GoalDTO> goalsMySql = GoalRepository.Instance.GetByEpisodeId(episodeId);

            List<GoalEngineDTO> goalsEngine = (from goal in goalsMySql
                                               select new GoalEngineDTO
                                               {
                                                   Goal = goal.Goal,
                                                   MetricIcon = goal.Icon,
                                                   MetricId = goal.ExternalMetricId,
                                                   MetricName = goal.MetricName,
                                                   RunId = goal.RunId,
                                                   Percentage = 0
                                               }).ToList();

            foreach (GoalEngineDTO goal in goalsEngine)
            {
                try
                {
                    GoalEngineDTO g = GoalEngineService.Instance.GetByRunIdAndMetricId(goal.RunId, goal.MetricId);
                    goal.Id = g.Id;
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }

                GoalEngineService.Instance.CreateOrUpdate(goal);
            }
        }
    }
}