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
    public class ScriptsMigration
    {
        public void MigrationGoalToEngine()
        {
            List<GameEngineDTO> games = GameEngineService.Instance.GetAll(0,10000).List.game;

            foreach(GameEngineDTO game in games)
            {
                List<EpisodeEngineDTO> episodes = EpisodeEngineService.Instance.GetAllByGameId(game.Id, 0, 10000).List.episode;

                List<MetricEngineDTO> metrics = MetricEngineService.Instance.GetAllByGameId(game.Id, 0, 10000).List.metric;

                foreach(EpisodeEngineDTO episode in episodes)
                {
                    MySqlGoalToEngineByEpisodeId(episode.Id, metrics);
                }
            }
        }

        public void MySqlGoalToEngineByEpisodeId(string episodeId, List<MetricEngineDTO> metrics)
        {
            List<GoalDTO> goalsMySql = GoalRepository.Instance.GetAllByEpisodeId(episodeId);

            List<GoalEngineDTO> goalsEngine = (from goal in goalsMySql
                                               select new GoalEngineDTO
                                               {
                                                   Goal = goal.Goal,
                                                   MetricIcon = (from metric in metrics where metric.Id == goal.ExternalMetricId select metric.Icon).FirstOrDefault(),
                                                   MetricId = goal.ExternalMetricId,
                                                   MetricName = (from metric in metrics where metric.Id == goal.ExternalMetricId select metric.Name).FirstOrDefault(),
                                                   RunId = goal.RunId,
                                                   Percentage = 0,
                                                   ItemId = ""
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