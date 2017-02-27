using System.Collections.Generic;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Public.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Controllers.Public.Model;
using System.Linq;
using Newtonsoft.Json;
using System;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Public
{
    [RoutePrefix("public/resultadosIndividuais")]
    [CustomAuthorize(Roles = "WORKER,LIDER,JOGADOR")]
    public class IndividualResultsController : BaseController
    {
        // GET: Dashboard
        [Route("")]
        public ActionResult Index(int state = 1)
        {
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, 1);
            ViewBag.Episodes = from episode in all.List.episode
                               select new SelectListItem
                               {
                                   Value = episode.Id.ToString(),
                                   Text = episode.Name
                               };

            ViewBag.State = state;


            return View("Index");
        }

        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarEpisodios")]
        [HttpGet]
        public ActionResult SearchEpisodes(int state)
        {
            List<EpisodeEngineDTO> episodes = EpisodeEngineService.Instance.EpisodesByPlayerId(CurrentWorker.ExternalId, CurrentFirm.ExternalId, state);

            return Json(JsonConvert.SerializeObject(episodes), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os times
        /// </summary>
        /// <returns></returns>
        [Route("buscarEquipes")]
        [HttpGet]
        public ActionResult SearchTeams(string episodeId)
        {
            List<TeamEngineDTO> teams = TeamEngineService.Instance.TeamsByPlayerIdAndEpisodeId(episodeId, CurrentWorker.ExternalId);

            return Json(JsonConvert.SerializeObject(teams), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os resultados filtrados
        /// </summary>
        /// <returns></returns>
        [Route("buscarResultados")]
        [HttpGet]
        public ActionResult SearchResults(string episodeId, string teamId)
        {
            List<CardEngineDTO> results = new List<CardEngineDTO>();

            if (teamId != "empty" && teamId != "")
            {
                results = CardEngineService.Instance.Player(CurrentFirm.ExternalId, teamId, CurrentWorker.ExternalId);
            }
            else
            {
                results = CardEngineService.Instance.Episode(CurrentFirm.ExternalId, episodeId);
            }

            return Json(JsonConvert.SerializeObject(results), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca as metricas de um tipo de funcionario.
        /// </summary>
        /// <returns></returns>
        [Route("buscarMetricas/{teamId:int}")]
        [HttpGet]
        public ActionResult GetMetricsTeam(int teamId)
        {
            List<MetricEntity> metrics = MetricRepository.Instance.GetMetricsTeamById(teamId);

            return Json(JsonConvert.SerializeObject(metrics), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tela de detalhes dos resultados de uma metrica.
        /// </summary>
        /// <returns></returns>
        [Route("detalhes/{episodeId}/{metricId}/{teamId}/{playerId}")]
        public ActionResult Details(string episodeId, string metricId, string teamId, string playerId)
        {
            MetricEngineDTO metric = MetricEngineService.Instance.GetById(metricId);
            metric.Icon = metric.Icon.Replace("_", "-");

            ViewBag.EpisodeId = episodeId;
            ViewBag.TeamId = teamId;
            ViewBag.PlayerId = playerId;

            if (playerId != "empty")
            {
                PlayerEngineDTO player = PlayerEngineService.Instance.GetById(playerId);
                ViewBag.Name = player.Nick;
            }
            else if (teamId != "empty")
            {
                TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);
                ViewBag.Name = team.Nick;
            }
            else
            {
                EpisodeEngineDTO episode = EpisodeEngineService.Instance.GetById(episodeId);
                ViewBag.Name = episode.Name;
            }

            return View("Detail", metric);
        }

        /// <summary>
        /// Popula uma tabela com infomaçoes de resultados lançados.
        /// </summary>
        /// <returns></returns>
        [Route("resultadosMetrica/{metricId}/{episodeId}/{teamId}/{playerId}")]
        public ActionResult SearchMetricResults(JQueryDataTableRequest jqueryTableRequest, string metricId, string episodeId, string teamId, string playerId)
        {

            if (jqueryTableRequest != null)
            {
                GetAllDTO all = new GetAllDTO();

                if (playerId != "" && playerId != "empty")
                {
                    RunEngineDTO run = RunEngineService.Instance.GetRunByPlayerAndTeamId(playerId, teamId);
                    all = RunMetricEngineService.Instance.findByRunIdAndMetricId(run.Id, metricId, jqueryTableRequest.Page);
                }
                else if (teamId != "" && teamId != "empty" && teamId != "null")
                {
                    all = TeamEngineService.Instance.resultsByTeamIdAndMetricId(teamId, metricId, jqueryTableRequest.Page);
                }
                else
                {
                    all = EpisodeEngineService.Instance.resultsByEpisodeIdAndMetricId(episodeId, metricId, jqueryTableRequest.Page);
                }

                List<WorkerDTO> workers = WorkerRepository.Instance.GetWorkerDTOByListExternalId(all.List.runMetric.Select(i => i.PlayerId).ToList());

                foreach (RunMetricEngineDTO rm in all.List.runMetric)
                {
                    DateTime dat = new DateTime(rm.Date);
                    string ds = dat.ToString("dd/MM/yyyy");
                }

                JQueryDataTableResponse response = new JQueryDataTableResponse()
                {
                    Draw = jqueryTableRequest.Draw,
                    RecordsTotal = all.PageInfo.totalElements,
                    RecordsFiltered = all.PageInfo.totalElements,
                    Data = (from runMetric in all.List.runMetric
                            join worker in workers on runMetric.PlayerId equals worker.ExternalId into runMetricWorker
                            from rmw in runMetricWorker.DefaultIfEmpty()
                            select new { Date = new DateTime(runMetric.Date), WorkerName = rmw != null ? rmw.Name : "Jogador excluído", CPF = rmw != null ? rmw.Cpf : "", Result = runMetric.Points, RunMetricId = runMetric.Id }).
                            Select(r => new string[] { r.Date.ToString("dd/MM/yyyy"), r.WorkerName, r.CPF, r.Result.ToString(), r.RunMetricId }).ToArray()
                };

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna um resultado para ser editado
        /// </summary>
        /// <returns></returns>
        [Route("editar/{runMetricId}")]
        public ActionResult Edit(string runMetricId)
        {
            RunMetricEngineDTO runMetric = RunMetricEngineService.Instance.GetById(runMetricId);

            return PartialView("_Edit", runMetric);
        }

        ///<summary>
        ///Salvar edição de resultado
        ///</summary>
        ///<returns></returns>
        [Route("editarResultado")]
        [HttpPost]
        public ActionResult EditResult(RunMetricEngineDTO runMetric, string date)
        {
            DateTime dateTime = Convert.ToDateTime(date);
            runMetric.Date = dateTime.Ticks;

            RunMetricEngineService.Instance.CreateOrUpdate(runMetric);

            return new EmptyResult();
        }

        ///<summary>
        ///Remover resultado
        ///</summary>
        ///<returns></returns>
        [Route("remover/{runMetricId}")]
        [HttpPost]
        public ActionResult Remove(string runMetricId)
        {
            RunMetricEngineService.Instance.DeleteById(runMetricId);

            return new EmptyResult();
        }

        #region Métodos privados

        private int CalculatesGoal(int totalGoal, long playersCount, bool IsAverage)
        {
            int goal;
            if (IsAverage)
            {
                goal = (int)(totalGoal / (float)playersCount);
            }
            else
            {
                goal = totalGoal;
            }

            return goal;
        }

        private float CalculatesPercentGoal(int totalGoal, int totalPoints, long playersCount, bool isAverage, bool isInverse)
        {
            float percentGoal;

            if (isAverage)
            {
                float averageGoal = totalGoal / (float)playersCount;
                percentGoal = isInverse ? averageGoal / (float)totalPoints : totalPoints / (float)Math.Round(averageGoal);
            }
            else
            {
                percentGoal = isInverse ? totalGoal / (float)totalPoints : totalPoints / (float)totalGoal;
            }

            return percentGoal;
        }

        #endregion

    }
}