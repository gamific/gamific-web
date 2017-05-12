using System.Collections.Generic;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
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
    [RoutePrefix("public/dashboard")]
    [CustomAuthorize(Roles = "WORKER,LIDER,ADMINISTRADOR")]

    public class DashboardController : BaseController
    {

        List<string> colorsToAdd = new List<string> { "#151F31", "#4D535A", "#0F65E7", "#AAC6DA", "#5789CA", "#1373C9", "#88C3BB", "#40ACFF",
                "#53B0A6", "#20614D", "#1B5B67", "#4D776D", "#FFA500", "#7FFFD4", "#87CEFA", "#FF69B4", "#FF00FF", "#7CFC00", "#BC8F8F", "#4682B4", "#006400",
            "#7B68EE", "#151F31", "#4D535A", "#0F65E7", "#AAC6DA", "#5789CA", "#1373C9", "#88C3BB", "#40ACFF" ,  "#151F31", "#4D535A", "#0F65E7", "#AAC6DA", "#5789CA", "#1373C9", "#88C3BB", "#40ACFF",
                "#53B0A6", "#20614D", "#1B5B67", "#4D776D", "#FFA500", "#7FFFD4", "#87CEFA", "#FF69B4", "#FF00FF", "#7CFC00", "#BC8F8F", "#4682B4", "#006400",
            "#7B68EE", "#151F31", "#4D535A", "#0F65E7", "#AAC6DA", "#5789CA", "#1373C9", "#88C3BB", "#40ACFF"};

        public static List<EpisodeEngineDTO> episodesFilter = new List<EpisodeEngineDTO>();

        // GET: Dashboard
        [Route("")]
        public ActionResult Index(int state = 1)
        {
            episodesFilter = new List<EpisodeEngineDTO>();
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, 1);
            ViewBag.Episodes = from episode in all.List.episode
                               select new SelectListItem
                               {
                                   Value = episode.Id.ToString(),
                                   Text = episode.Name
                               };

            ViewBag.Grafic_itens = changeVisibilityGraph();

            //ViewBag.Metrics = MetricEngineService.Instance.GetByGameId(CurrentFirm.ExternalId).List.metric;

            ViewBag.Metrics = MetricEngineService.Instance.GetAllDTOByGame(CurrentFirm.ExternalId, 0, 100).List.metric;

            ViewBag.State = state;

            return View("Index");
        }

        private bool changeVisibilityGraph()
        {
            bool active = false;

            ParamEntity grafics = ParamRepository.Instance.GetElementParam(CurrentFirm.ExternalId, ParamEntity.GRAFICO_PRODUTOS);

            if (grafics != null && grafics.Value == "1")
            {
                active = true;
            }

            return active;

        }

        [Route("getCampaignsWithIds")]
        [HttpGet]
        public ContentResult GetCampaignsWithIds()
        {
            List<EpisodeEngineDTO> episodes = new List<EpisodeEngineDTO>();

            if (episodesFilter.Count < 1)
            {
                GetCampaignsModal();
            }

            foreach (EpisodeEngineDTO episode in episodesFilter)
            {
                if (episodes.Count > 7)
                {
                    break;
                }
                else
                {
                    if (episode.checkedFlag)
                    {
                        episodes.Add(episode);
                    }
                }
            }

            return Content(JsonConvert.SerializeObject(episodes), "application/json");
        }

        [Route("getCampaignsModal")]
        [HttpGet]
        public ActionResult GetCampaignsModal()
        {

            List<EpisodeEngineDTO> episodesRtn = new List<EpisodeEngineDTO>();

            if (episodesFilter.Count > 0)
            {
                episodesRtn = episodesFilter;
            }
            else
            {
                episodesRtn = EpisodeEngineService.Instance.GetByGameId(CurrentFirm.ExternalId, 0, 1000).List.episode;

                int i = 0;
                foreach (EpisodeEngineDTO episode in episodesRtn)
                {

                    if (episodesFilter.Count < 7)
                    {
                        episode.checkedFlag = true;
                    }

                    episodesFilter.Add(episode);

                    i++;
                }

            }

            return PartialView("_CampaignsFilter", episodesRtn);
        }

        [Route("keepCampaigns")]
        public void KeepCampaigns(List<EpisodeEngineDTO> episodes)
        {
            episodesFilter = episodes;
        }

        [Route("getCampaigns")]
        [HttpGet]
        public ContentResult GetCampaigns()
        {
            List<string> rtn = new List<string>();

            if (episodesFilter.Count < 1)
            {
                GetCampaignsModal();
            }

            foreach (EpisodeEngineDTO episode in episodesFilter)
            {
                if (rtn.Count > 7)
                {
                    break;
                }
                else
                {
                    if (episode.checkedFlag)
                    {
                        rtn.Add(episode.Name);
                    }
                }
            }

            return Content(JsonConvert.SerializeObject(rtn), "application/json");
        }

        [Route("loadBarChart")]
        [HttpPost]
        public ContentResult LoadBarChart(List<string> metricsIds)
        {

            if (episodesFilter.Count < 1)
            {
                GetCampaignsModal();
            }

            List<BarDTO> bars = new List<BarDTO>();

            List<string> episodesIds = new List<string>();

            List<MetricEngineDTO> metrics = new List<MetricEngineDTO>();

            foreach (string item in metricsIds)
            {
                metrics.Add(MetricEngineService.Instance.GetById(item));
            }

            List<EpisodeEngineDTO> episodesParam = new List<EpisodeEngineDTO>();

            foreach (EpisodeEngineDTO item in episodesFilter)
            {
                if (item.checkedFlag)
                {
                    episodesParam.Add(item);
                }
            }

            bars = CardEngineService.Instance.EpisodesAndMetrics(episodesParam, metrics);

            return Content(JsonConvert.SerializeObject(bars), "application/json");
        }

        [Route("loadMorrisByEpisode/{metricId}/{episodeId}")]
        [HttpGet]
        public ContentResult LoadMorrisByEpisode(string metricId, string episodeId)
        {
            MorrisDTO dto = new MorrisDTO
            {
                products = new List<MorrisPropertyDTO>()
            };


            List<ItemEngineDTO> items = new List<ItemEngineDTO>();

            items = ItemEngineService.Instance.FindByEpisode(metricId, episodeId);

            int i = 0;
            List<string> colors = new List<string>();

            foreach (ItemEngineDTO item in items)
            {
                if (item.Name != null)
                {
                    MorrisPropertyDTO morrisDTO = new MorrisPropertyDTO();

                    morrisDTO.label = item.Name;
                    morrisDTO.label2 = item.Name;
                    morrisDTO.value = double.Parse(item.Value.ToString("n2"));

                    colors.Add(colorsToAdd[i]);

                    dto.products.Add(morrisDTO);

                    if (i > colorsToAdd.Count)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            dto.colors = colors;

            return Content(JsonConvert.SerializeObject(dto), "application/json");
        }

        [Route("loadMorrisByRun/{metricId}/{playerId}/{teamId}")]
        [HttpGet]
        public ContentResult LoadMorrisByRun(string metricId, string playerId, string teamId)
        {
            MorrisDTO dto = new MorrisDTO
            {
                products = new List<MorrisPropertyDTO>()
            };


            List<ItemEngineDTO> items = new List<ItemEngineDTO>();

            string runId;

            runId = RunEngineService.Instance.GetRunByPlayerAndTeamId(playerId, teamId).Id;

            items = ItemEngineService.Instance.FindByRun(metricId, runId);

            int i = 0;
            List<string> colors = new List<string>();

            foreach (ItemEngineDTO item in items)
            {
                if (item.Name != null)
                {
                    MorrisPropertyDTO morrisDTO = new MorrisPropertyDTO();

                    morrisDTO.label = item.Name;
                    morrisDTO.label2 = item.Name;
                    morrisDTO.value = double.Parse(item.Value.ToString("n2"));

                    colors.Add(colorsToAdd[i]);

                    dto.products.Add(morrisDTO);

                    if (i > colorsToAdd.Count)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            dto.colors = colors;

            return Content(JsonConvert.SerializeObject(dto), "application/json");
        }

        [Route("loadMorrisByTeam/{metricId}/{teamId}")]
        [HttpGet]
        public ContentResult LoadMorrisByTeam(string metricId, string teamId)
        {
            MorrisDTO dto = new MorrisDTO
            {
                products = new List<MorrisPropertyDTO>()
            };


            List<ItemEngineDTO> items = new List<ItemEngineDTO>();

            items = ItemEngineService.Instance.FindByTeam(metricId, teamId);

            int i = 0;
            List<string> colors = new List<string>();

            foreach (ItemEngineDTO item in items)
            {
                if (item.Name != null)
                {
                    MorrisPropertyDTO morrisDTO = new MorrisPropertyDTO();

                    morrisDTO.label = item.Name;
                    morrisDTO.label2 = item.Name;
                    morrisDTO.value = double.Parse(item.Value.ToString("n2"));

                    colors.Add(colorsToAdd[i]);

                    dto.products.Add(morrisDTO);

                    if (i >= colorsToAdd.Count)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            dto.colors = colors;

            return Content(JsonConvert.SerializeObject(dto), "application/json");
        }

        [Route("loadChart")]
        [HttpPost]
        public ContentResult GetChartResults(List<string> metricsIds, string campaignId, string initDate, string endDate)
        {

            List<ChartResultDTO> rtn = new List<ChartResultDTO>();

            EpisodeEngineDTO episodeObj = EpisodeEngineService.Instance.GetById(campaignId);

            List<string> episodesParam = new List<string>();

            episodesParam.Add(campaignId);

            DateTime initDT = DateTime.Parse(initDate);

            DateTime endDT = DateTime.Parse(endDate);

            foreach (string item in metricsIds)
            {
                ChartResultDTO chartDTO = new ChartResultDTO();

                List<string> metricParam = new List<string>();

                metricParam.Add(item);

                chartDTO = CardEngineService.Instance.GameAndMetricAndPeriod(episodesParam, episodeObj.GameId, metricParam, initDT.Ticks, endDT.Ticks);

                rtn.Add(chartDTO);
            }

            return Content(JsonConvert.SerializeObject(rtn), "application/json");
        }

        /*
        [Route("loadChart/{metricId}")]
        [HttpGet]
        public ContentResult GetChartResults(string metricId)
        {
            ChartResultDTO chartDTO = new ChartResultDTO();

            chartDTO.Positions = new List<List<int>>();

            MetricEngineDTO metric = MetricEngineService.Instance.GetById(metricId);

            List<EpisodeEngineDTO> episodes = EpisodeEngineService.Instance.GetByGameId(CurrentFirm.ExternalId, 0, 8).List.episode;

            int i = 0;

            foreach (EpisodeEngineDTO episode in episodes)
            {
                List<int> point = new List<int>();
                
                List<CardEngineDTO> results = new List<CardEngineDTO>();
                List<GoalDTO> goals = new List<GoalDTO>();
                results.Add(CardEngineService.Instance.EpisodeAndMetric(episode.Id, metric.Id));
                goals = GoalRepository.Instance.GetByEpisodeId(episode.Id);
                long playersCount = EpisodeEngineService.Instance.GetCountPlayersByEpisodeId(episode.Id);

                results   = (from result in results
                        join goal in goals
                        on result.MetricId equals goal.ExternalMetricId into rg
                        from resultGoal in rg.DefaultIfEmpty()
                        select new CardEngineDTO
                        {
                            IconMetric = result.IconMetric.Replace("_", "-"),
                            MetricId = result.MetricId,
                            MetricName = result.MetricName,
                            TotalPoints = result.TotalPoints,
                            Goal = (resultGoal != null ? CalculatesGoal(resultGoal.Goal, playersCount, result.IsAverage) : 0),
                            PercentGoal = (resultGoal != null && resultGoal.Goal != 0 ? CalculatesPercentGoal(resultGoal.Goal, result.TotalPoints, playersCount, result.IsAverage, result.IsInverse) : 0),
                            IsAverage = result.IsAverage
                        }).ToList();

                int resultInt = 0;

                if (results != null)
                {
                    foreach (CardEngineDTO result in results)
                    {
                        resultInt += result.TotalPoints;
                    }
                }

                point.Add(i);
                point.Add(resultInt);
                chartDTO.Positions.Add(point);
                i++;
            }

            chartDTO.MetricName = metric.Name;

            return Content(JsonConvert.SerializeObject(chartDTO), "application/json");
        }
        */

        // GET: Dashboard
        [Route("{episodeId}/{teamId}/{playerId}")]
        public ActionResult Index(string episodeId, string teamId, string playerId)
        {
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, 1);
            ViewBag.Episodes = from ep in all.List.episode
                               select new SelectListItem
                               {
                                   Value = ep.Id.ToString(),
                                   Text = ep.Name
                               };

            EpisodeEngineDTO episode = EpisodeEngineService.Instance.GetById(episodeId);

            //ViewBag.Metrics = MetricEngineService.Instance.GetByGameId(CurrentFirm.ExternalId).List.metric;
            ViewBag.Metrics = MetricEngineService.Instance.GetAllDTOByGame(CurrentFirm.ExternalId, 0, 100).List.metric;
            ViewBag.State = episode.Active == true ? 1 : 0;
            ViewBag.EpisodeId = episodeId;
            ViewBag.TeamId = teamId;
            ViewBag.PlayerId = playerId;
            ViewBag.Grafic_itens = changeVisibilityGraph();


            return View("Index");
        }

        /// <summary>
        /// Preenche os campos automaticamente quando voltamos da tela de detalhes de uma metrica
        /// </summary>
        /// <returns></returns>
        [Route("{teamId:int}/{workerId:int}/{workerTypeId:int}")]
        public ActionResult Index(int teamId, int workerId, int workerTypeId)
        {
            PlayerEngineDTO player = PlayerEngineService.Instance.GetById("58ada8b13001c12f60c1460f");

            List<WorkerTypeEntity> workerTypes = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.Id);
            ViewBag.WorkerTypes = from workerType in workerTypes
                                  select new SelectListItem
                                  {
                                      Value = workerType.Id.ToString(),
                                      Text = workerType.TypeName
                                  };

            DateTime endDate = DateTime.Now.AddDays(1);
            DateTime initialDate = endDate.AddMonths(-1);

            FilterResultDTO filter = new FilterResultDTO { InitialDate = initialDate.ToString("dd/MM/yyyy"), EndDate = endDate.ToString("dd/MM/yyyy") };

            if (workerTypeId != 0)
            {
                ViewBag.WorkerTypeId = workerTypeId.ToString();

                List<TeamEntity> teams = TeamRepository.Instance.GetAllFromWorkerType(workerTypeId);
                ViewBag.Teams = from team in teams
                                select new SelectListItem
                                {
                                    Value = team.Id.ToString(),
                                    Text = team.TeamName
                                };

                if (teamId != 0)
                {
                    ViewBag.TeamId = teamId.ToString();

                    List<WorkerDTO> workers = WorkerRepository.Instance.GetAllFromTeam(teamId);

                    ViewBag.Workers = from worker in workers
                                      select new SelectListItem
                                      {
                                          Value = worker.IdWorker.ToString(),
                                          Text = worker.Name
                                      };

                    if (workerId != 0)
                    {
                        ViewBag.WorkerId = workerId.ToString();
                    }
                }
            }


            return View("Index", filter);
        }

        /// <summary>
        /// Busca os jogadores de um time
        /// </summary>
        /// <returns></returns>
        [Route("buscarJogadores")]
        [HttpGet]
        public ActionResult SearchPlayers(string teamId)
        {
            GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId, 0, 10000);

            List<string> externalIds = (from run in all.List.run select run.PlayerId).ToList();

            List<WorkerDTO> workers = WorkerRepository.Instance.GetDTOFromListExternalId(externalIds);

            List<SelectListItem> workersList = (from worker in workers
                                                select new SelectListItem
                                                {
                                                    Value = worker.ExternalId,
                                                    Text = worker.Name
                                                }).ToList();

            return Json(JsonConvert.SerializeObject(workersList), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarEpisodios")]
        [HttpGet]
        public ActionResult SearchEpisodes(int state)
        {
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, state);

            return Json(JsonConvert.SerializeObject(all.List.episode), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os times
        /// </summary>
        /// <returns></returns>
        [Route("buscarEquipes")]
        [HttpGet]
        public ActionResult SearchTeams(string episodeId)
        {
            GetAllDTO all = TeamEngineService.Instance.FindByEpisodeId(episodeId);

            return Json(JsonConvert.SerializeObject(all.List.team), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os times de um tipo de jogador
        /// </summary>
        /// <returns></returns>
        [Route("buscarTimes")]
        [HttpGet]
        public ActionResult SearchTeams(int workerTypeId)
        {
            List<TeamEntity> teams = TeamRepository.Instance.GetAllFromWorkerType(workerTypeId);

            return Json(JsonConvert.SerializeObject(teams), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os resultados filtrados
        /// </summary>
        /// <returns></returns>
        [Route("buscarResultados")]
        [HttpGet]
        public ActionResult SearchResults(string episodeId, string teamId, string playerId)
        {
            List<CardEngineDTO> results = new List<CardEngineDTO>();

            if (playerId != "empty" && playerId != "")
            {
                results = CardEngineService.Instance.Player(CurrentFirm.ExternalId, teamId, playerId);
            }
            else if (teamId != "empty" && teamId != "")
            {
                results = CardEngineService.Instance.Team(CurrentFirm.ExternalId, teamId);
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

                List<WorkerDTO> workers = all.List == null ? new List<WorkerDTO>() : WorkerRepository.Instance.GetWorkerDTOByListExternalId(all.List.runMetric.Select(i => i.PlayerId).ToList());
                GetAllDTO itens = ItemEngineService.Instance.GetByGameId(CurrentFirm.ExternalId, 1000, 0);


                if (all.List != null)
                {
                    foreach (RunMetricEngineDTO rm in all.List.runMetric)
                    {
                        DateTime dat = new DateTime(rm.Date);
                        string ds = dat.ToString("dd/MM/yyyy");
                    }
                }

                JQueryDataTableResponse response = new JQueryDataTableResponse()
                {
                    Draw = jqueryTableRequest.Draw,
                    RecordsTotal = all.PageInfo.totalElements,
                    RecordsFiltered = all.PageInfo.totalElements,
                    Data = (from runMetric in all.List.runMetric
                            join worker in workers on runMetric.PlayerId equals worker.ExternalId into runMetricWorker
                            from rmw in runMetricWorker.DefaultIfEmpty()
                            select new { Date = new DateTime(runMetric.Date), WorkerName = rmw != null ? rmw.Name : "Jogador excluído", Email = rmw != null ? rmw.Email : "", Result = runMetric.Points, RunMetricId = runMetric.Id, ItemName = itens.List == null ? "" : (itens.List.item.Where(q => q.Id == runMetric.ItemId).Select(x => x.Name)).FirstOrDefault() }).
                            Select(r => new string[] { r.Date.ToString("dd/MM/yyyy"), r.WorkerName, r.Email, r.ItemName, r.Result.ToString(), r.RunMetricId }).ToArray()
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