using System;
using Gamific.Scheduler;
using System.Threading;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Util.Parameter;
using Vlast.Broker.EMAIL;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using System.Collections;
using System.Globalization;
using Vlast.Gamific.Web.Controllers.Util;

namespace Vlast.Gamific.Web.Jobs
{
    /// <summary>
    /// Envia, periodicamente, por email o ranking geral para os jogadores.
    /// </summary>
    public class RankingMailThread : BaseThread
    {
        private string email = "rubens";
        /// <summary>
        /// Envia por email o ranking geral para os jogadores.
        /// </summary>
        /// <returns></returns>
        public async override void Run()
        {

            //Send(new EmailSupportDTO { Msg = "Bom dia", Category = "", Subject = "Testes Gamific" }, "igorgarantes@gmail.com");

            //GetAllDTO players = PlayerEngineService.Instance.GetByGameId(game.Id);

            //GetAllDTO episodes = EpisodeEngineService.Instance.GetByGameIdAndActive(game.Id, 1);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            string dayOfWeek = DateTime.Now.ToString("ddd");
            List<EpisodeEngineDTO> episodes;
            //episodes.Add(new EpisodeEngineDTO("58e3708a3a8778588098a82b", "5880a1743a87783b4f0ba709"));

            GetAllDTO allEpisodes = EpisodeEngineService.Instance.GetAll(0, 100000);

            episodes = allEpisodes.List.episode.Where(x => x.Active == true && x.sendEmail == true && (x.DaysOfWeek != null ? x.DaysOfWeek.Split(',').Contains(dayOfWeek.ToLower()) : false)).ToList();

            foreach (EpisodeEngineDTO episode in episodes)
            {
                List<string> emails = new List<string>();
                GetAllDTO teams = TeamEngineService.Instance.FindByEpisodeId(episode.Id, email);
                GameEngineDTO game = GameEngineService.Instance.GetById(episode.GameId, email);

                foreach (TeamEngineDTO team in teams.List.team)
                {
                    if(team.SubTeams == null || team.SubTeams.Count() == 0)
                    {
                        GetAllDTO runs = RunEngineService.Instance.GetRunsByTeamId(team.Id, email);

                        foreach (RunEngineDTO run in runs.List.run)
                        {
                            WorkerDTO worker = WorkerRepository.Instance.GetWorkerDTOByExternalId(run.PlayerId);
                            if (worker != null && (worker.ProfileName == Profiles.LIDER || worker.ProfileName == Profiles.JOGADOR))
                            {
                                string emailBody = CreateEmail(game, episode.Id, team.Id, worker.ExternalId, worker);
                                //Send(new EmailSupportDTO { Msg = emailBody, Category = "", Subject = "Ranking Gamific" }, "igorgarantes@gmail.com");
                                //Send(new EmailSupportDTO { Msg = emailBody, Category = "", Subject = "Ranking Gamific" }, worker.Email);
                                EmailSender.Send(new EmailSupportDTO { Msg = emailBody, Category = "", Subject = "Ranking Gamific" }, worker.Email, game.Id, episode.Id, worker.ExternalId);
                            }
                        }
                    }
                }
            }
        }

        public override void Init(TimeSpan timeToRun)
        {
            RankingMailThread rankingThread = new RankingMailThread();

            rankingThread.timeToRun = timeToRun;

            Instance = new Thread(rankingThread.Start);
            Instance.Start();
        }

        private string CreateTable(List<ResultEngineDTO> results, string id)
        {
            string emailBody = "<table>";
            int i = 1;
            foreach (ResultEngineDTO result in results)
            {
                string score = result.Score.ToString("0,#");

                if (result.Id == id)
                {
                    emailBody += "<tr style='font-family: Roboto, sans-serif;'>";
                    emailBody += "<td><img src = 'https://s3.amazonaws.com/gamific-prd/images/logos/empresas/logo-" + result.LogoId + "' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' /></td>";
                    emailBody += "<td><strong>" + i + "°</strong></td>";
                    emailBody += "<td><strong>" + result.Nick + "</strong></td>";
                    emailBody += "<td  style='padding-left: 20px;'><strong>" + score + "</strong></td>";
                    emailBody += "</tr>";
                }
                else
                {
                    emailBody += "<tr style='font-family: Roboto, sans-serif;'>";
                    emailBody += "<td><img src = 'https://s3.amazonaws.com/gamific-prd/images/logos/empresas/logo-" + result.LogoId + "' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' /></td>";
                    emailBody += "<td>" + i + "°</td>";
                    emailBody += "<td>" + result.Nick + "</td>";
                    emailBody += "<td  style='padding-left: 20px;'>" + score + "</td>";
                    emailBody += "</tr>";
                }

                i++;
            }

            emailBody += "</table>";
            return emailBody;
        }

        private string CreateIndividualResultsTable(TeamEngineDTO team, PlayerEngineDTO player, string gameId, Profiles perfil)
        {
            RunEngineDTO run;
            List<GoalEngineDTO> goals = new List<GoalEngineDTO>();
            List<CardEngineDTO> results;
            if (perfil == Profiles.JOGADOR)
            {
                results = CardEngineService.Instance.PlayerWithEmail(gameId, team.Id, player.Id, email);
                run = RunEngineService.Instance.GetRunByPlayerAndTeamId(player.Id, team.Id, email);
                goals = GoalEngineService.Instance.GetByRunId(run.Id, email).List.goal;//GoalRepository.Instance.GetByRunId(run.Id);
            }
            else if(perfil == Profiles.LIDER)
            {
                results = CardEngineService.Instance.TeamWithEmail(gameId, team.Id, email);
                GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(team.Id, email);
                List<string> runIds = all.List.run.Select(x => x.Id).ToList();
                foreach(string runId in runIds)
                {
                    goals.AddRange(GoalEngineService.Instance.GetByRunId(runId, email).List.goal); //GoalRepository.Instance.GetByRunId(runIds);
                }
                
            }
            else
            {
                results = new List<CardEngineDTO>();
                run = new RunEngineDTO();
                goals = new List<GoalEngineDTO>();
            }
           

            results = (from result in results
                       
                       select new CardEngineDTO
                       {
                           IconMetric = result.IconMetric.Replace("_", "-"),
                           MetricId = result.MetricId,
                           MetricName = result.MetricName,
                           TotalPoints = result.TotalPoints,
                           Goal = result.Goal,
                           PercentGoal = result.PercentGoal,
                           IsAverage = result.IsAverage
                       }).ToList();


            string emailBody = "<table align='center' cellpadding = '5px'><tr>";
            emailBody += "<td></td><td>Objetivo</td><td>Alcançado</td><td>Porcentagem</td>";
            emailBody += "</tr>";

            foreach (CardEngineDTO result in results)
            {
                if(result.PercentGoal < 0.3f)
                {
                    emailBody += "<tr class='alert-goal'>";
                }
                else if(result.PercentGoal < 1.0f)
                {
                    emailBody += "<tr class='normal-goal'>";
                }
                else
                {
                    emailBody += "<tr class='sucess-goal'>";
                }

                emailBody += "<td class='name'>" + result.MetricName + "</td>";
                emailBody += "<td>" + result.Goal.ToString("###,###,###,###") + "</td>";
                emailBody += "<td>" + result.TotalPoints.ToString("###,###,###,###") + "</td>";
                emailBody += "<td>" + (result.PercentGoal * 100) + "%</td>";
                emailBody += "</tr>";
            }

            emailBody += "</table>";

            return emailBody;
        }

        private string CreateRankingPlayersFromTeamResultsTable(List<ResultEngineDTO> results, string playerId)
        {
            if(results == null)
            {
                return "";
            }

            string emailBody = "<table align='center' cellpadding='5px'>";
            int i = 1;
            foreach(ResultEngineDTO result in results)
            {
                emailBody += "<tr>";

                if (result.Id == playerId)
                {
                    emailBody += "<td><strong>" + i + "°</strong></td>";
                    emailBody += "<td class='name'><strong>" + result.Nick + "</strong></td>";
                    emailBody += "<td class='score'><strong>" + result.Score + "</strong></td>";
                }
                else
                {
                    emailBody += "<td>" + i + "°</td>";
                    emailBody += "<td class='name'>" + result.Nick + "</td>";
                    emailBody += "<td class='score'>" + result.Score + "</td>";
                }

                emailBody += "</tr>";
                i++;              
            }

            emailBody += "</table>";

            return emailBody;
        }

        private string CreateRankingTeamsFromCampaignResultsTable(List<ResultEngineDTO> results, string teamId)
        {
            if(results == null)
            {
                return null;
            }

            string emailBody = "<table align='center' cellpadding='5px'>";
            int i = 1;
            foreach (ResultEngineDTO result in results)
            {
                emailBody += "<tr>";

                if (result.Id == teamId)
                {
                    emailBody += "<td><strong>" + i + "°</strong></td>";
                    emailBody += "<td class='name'><strong>" + result.Nick + "</strong></td>";
                    emailBody += "<td class='score'><strong>" + result.Score + "</strong></td>";
                }
                else
                {
                    emailBody += "<td>" + i + "°</td>";
                    emailBody += "<td class='name'>" + result.Nick + "</td>";
                    emailBody += "<td class='score'>" + result.Score + "</td>";
                }

                emailBody += "</tr>";
                i++;
            }

            emailBody += "</table>";

            return emailBody;
        }

        private string CreateEmail(GameEngineDTO game, string episodeId, string teamId, string playerId, WorkerDTO worker)
        {
            PlayerEngineDTO player = playerId == null ? null : PlayerEngineService.Instance.GetById(playerId, email);
            TeamEngineDTO team = teamId == null ? null : TeamEngineService.Instance.GetById(teamId, email);
            EpisodeEngineDTO episode = episodeId == null ? null : EpisodeEngineService.Instance.GetById(episodeId, email);
            GetAllDTO runs = RunEngineService.Instance.GetAllRunScore(teamId, "", email);
            GetAllDTO teams = TeamEngineService.Instance.GetAllTeamScoreByEpisodeId(episode.Id, "", email, 0, 10);

            if (runs.List != null)
            {
                List<PlayerEngineDTO> players = new List<PlayerEngineDTO>();

                foreach (RunEngineDTO run in runs.List.run)
                {
                    try
                    {
                        PlayerEngineDTO p = PlayerEngineService.Instance.GetById(run.PlayerId, email);
                        if (p != null)
                        {
                            players.Add(p);
                        }
                    }
                    catch (Exception e)
                    {

                    }

                }

                runs.List.result = (from run in runs.List.run
                                    from p in players
                                    where p.Id == run.PlayerId
                                    select new ResultEngineDTO
                                    {
                                        Id = p.Id,
                                        Nick = p.Nick,
                                        Score = run.Score,
                                        LogoId = p.LogoId
                                    }).ToList();

                if (player.Id != null && worker.ProfileName == Profiles.JOGADOR && runs.List.result.Find(x => x.Id == player.Id) == null)
                {
                    RunEngineDTO playerRun = RunEngineService.Instance.GetRunByPlayerAndTeamId(player.Id, team.Id, email);
                    ResultEngineDTO result = new ResultEngineDTO
                    {
                        Id = player.Id,
                        Nick = player.Nick,
                        Score = playerRun.Score,
                        LogoId = player.LogoId
                    };

                    runs.List.result.Add(result);
                }
            }

            if (teams.List != null)
            {
                teams.List.result = (from t in teams.List.team
                                     select new ResultEngineDTO
                                     {
                                         Id = t.Id,
                                         Nick = t.Nick,
                                         Score = t.Score,
                                         LogoId = t.LogoId
                                     }).ToList();
            }

            string emailBody = "<!DOCTYPE html><html xmlns = 'http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8;'/><style>body{color: #000000; font-family: Roboto, sans-serif;} h1{font-size: 30px;} h2{font-size: 24px;}h3{font-size: 18px;} strong{color: #1cb29b;} a{text-decoration: none; color: #153643; font-weight: 700;} td{text-align: center;} .image-icon{width:48px;height:48px !important;border-radius:100%;margin-right: 10px;} .score{text-align: right !important; padding-left:15px;} .name{text-align:left !important;} .sucess-goal{color: #1cb29b !important; font-weight: bold;} .alert-goal{color: firebrick;font-weight: bold;} .normal-goal{color: orange; font-weight:bold;}</style></head>";
            emailBody += "<body style = 'margin: 0; padding: 0;'><table align = 'center' border = '0' cellpadding = '0' cellspacing = '0' width = '1000'>";
            emailBody += "</td></tr><tr><td style='padding: 0 0 0 0;' align='center'>";
            emailBody += "</td></tr><tr><td>";
            emailBody += "<table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td width='260' valign='top'>";
            emailBody += "<tr><td align='center'><img src = 'https://s3.amazonaws.com/gamific-prd/images/logos/empresas/logo-1' width = '340' height = '223'/></td></tr>";
            emailBody += "<tr><td><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td><h1 align='center'> Olá " + player.Nick + "!<br/> Aqui estão seus resultados diários</h1></td></tr>";
            emailBody += "<tr><td><h2 align='center'> Campanha " + episode.Name + " </h2></td></tr>";
            emailBody += "<tr><td><h2 align='center'>" + team.Nick + " </h2></td></tr>";
            emailBody += "<tr><td><h3 align='center'> Resultados individuais </h3>";
            emailBody += CreateIndividualResultsTable(team, player, game.Id, worker.ProfileName);
            emailBody += "<h3 align='center'>Ranking dos membros da equipe</h3>";
            emailBody += CreateRankingPlayersFromTeamResultsTable(runs.List != null ? runs.List.result : null, playerId);
            emailBody += "<h3 align='center'>Ranking das equipes na campanha</h3>";
            emailBody += CreateRankingTeamsFromCampaignResultsTable(teams.List != null ? teams.List.result : null , teamId);
            emailBody += "</td></tr></table></td></tr>";
            emailBody += "<tr><td><p align='center'>Acesse o site: <a href='http://www.gamific.com.br/'> Gamific </a></p></td></tr>";
            emailBody += "</table></body></html>";
            
            return emailBody;
        }

       
    }
}

