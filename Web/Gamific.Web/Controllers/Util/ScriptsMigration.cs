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
    public class ScriptsMigration : BaseController
    {
        public void MigrationGoalToEngine()
        {
            List<GameEngineDTO> games = GameEngineService.Instance.GetAll(0,10000).List.game;

            foreach(GameEngineDTO game in games)
            {
                List<EpisodeEngineDTO> episodes = EpisodeEngineService.Instance.GetByGameId(game.Id, 0, 10000).List.episode;

                List<MetricEngineDTO> metrics = MetricEngineService.Instance.GetByGameId(game.Id, 0, 10000).List.metric;

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

        static public void MigrationEmailToEngine()
        {

            List<DataEntity> entityList = DataRepository.Instance.GetAll(0,100);


            foreach (DataEntity entity in entityList) { 

                GameEngineDTO game = new GameEngineDTO
            {
                Adress = entity.Adress,
                City = entity.City,
                LogoId = entity.LogoId,
                Name = entity.FirmName,
                Neighborhood = entity.Neighborhood,
                Phone = entity.Phone,
                Id = entity.ExternalId,
                LogoPath = CurrentURL + entity.LogoId
            };
            game = GameEngineService.Instance.CreateOrUpdate(game, "miller@gamific.com.br" );

            //List<WorkerDTO> workers = WorkerRepository.Instance.GetAllFromFirm(entity.ExternalId);

            List<WorkerDTO> workers = WorkerRepository.Instance.GetWorkerDTOByExternalGameId(entity.ExternalId);
            string errors = "";

            foreach(WorkerDTO worker in workers)
            {
                try
                {
                    PlayerEngineDTO player = new PlayerEngineDTO();
                    player.Email = worker.Email;
                    player.Cpf = worker.Cpf;
                    player.Role = worker.Role;
                    player.LogoId = worker.LogoId;
                    player.LogoPath = CurrentURL + player.LogoId;
                    player.Id = worker.ExternalId;
                    player.GameId = entity.ExternalId;
                    player.Nick = worker.Name;
                    player.Xp = 1;
                    player.Level = 1;
                    PlayerEngineService.Instance.CreateOrUpdate(player, "miller@gamific.com.br");
                }
                catch(Exception e)
                {
                    errors += worker.Email + " -> " + e.Message + "<br/>";
                }
            }

            }
        }
    }
}