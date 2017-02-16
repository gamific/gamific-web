using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Public.Model;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.BIZ;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Public
{
    [CustomAuthorize]
    [RoutePrefix("public/ranking")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,LIDER,JOGADOR")]
    public class RankingController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        { 
            ViewBag.Profile = CurrentWorkerType.ProfileName.ToString();

            return View();
        }

        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarEpisodios")]
        [HttpGet]
        public ActionResult SearchEpisodes()
        {
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, 1);

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
        /// Busca os times
        /// </summary>
        /// <returns></returns>
        [Route("buscarMetricas")]
        [HttpGet]
        public ActionResult SearchMetrics()
        {
            GetAllDTO all = MetricEngineService.Instance.GetByGameId(CurrentFirm.ExternalId);

            return Json(JsonConvert.SerializeObject(all.List.metric), JsonRequestBehavior.AllowGet);
        }

        //[Route("")]
        [HttpGet]
        public ActionResult Search(string episodeId, string teamId)
        {
            GetAllDTO all = new GetAllDTO();

            if (teamId != "")
            {
                //all = PlayerEngineService.Instance.
            }
            else
            {
                //all = TeamEngineService.Instance.GetAllTeamScoreByEpisodeId(episodeId);
            }
            

            return Json(JsonConvert.SerializeObject(all), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega um datatable com todas as equipes do episodio ordenados por Score
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <returns></returns>
        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, string episodeId, string teamId, string metricId)
        {
            GetAllDTO all;

            if(metricId == "empty")
            {
                metricId = "";
            }

            if (jqueryTableRequest != null)
            {
                if (teamId != "empty")
                {
                    all = RunEngineService.Instance.GetAllRunScore(teamId, metricId, jqueryTableRequest.Page);
                    List<PlayerEngineDTO> players = (from run in all.List.run select PlayerEngineService.Instance.GetById(run.PlayerId)).ToList();

                    all.List.result = (from run in all.List.run
                                       from player in players
                                       where run.PlayerId == player.Id
                                       select new ResultEngineDTO
                                       {
                                           Id = player.Id,
                                           Nick = player.Nick,
                                           Score = run.Score,
                                           LogoId = player.LogoId
                                       }).ToList();
                }
                else
                {
                    all = TeamEngineService.Instance.GetAllTeamScoreByEpisodeId(episodeId, metricId, jqueryTableRequest.Page);
                    all.List.result = (from team in all.List.team
                                       select new ResultEngineDTO
                                       {
                                           Id = team.Id,
                                           Nick = team.Nick,
                                           Score = team.Score,
                                           LogoId = team.LogoId
                                       }).ToList();
                }
                
                int index = 0;
                if (jqueryTableRequest.Order != null)
                {
                    index = Int32.Parse(jqueryTableRequest.Order);
                }
                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.result.Select(t => new string[] { jqueryTableRequest.Page.ToString(), t.Nick + ";" + t.LogoId + ";" + t.Id, t.Score.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.result.Select(t => new string[] { jqueryTableRequest.Page.ToString(), t.Nick + ";" + t.LogoId + ";" + t.Id, t.Score.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        /////////////////////////////////////////////////////////////////////////

        /*
        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.WorkerTypes = GetWorkerTypesToSelect(0);
            ViewBag.Teams = new List<SelectListItem>();
            ViewBag.Episodes = GetEpisodesToSelect("");

            RankingFilterDTO filter = new RankingFilterDTO();

            return View(filter);
        }
        */

        //[Route("search")]
        [HttpPost]
        public ActionResult Index(RankingFilterDTO filter)
        {
            ViewBag.WorkerTypes = GetWorkerTypesToSelect(filter.WorkerTypeId);
            ViewBag.Episodes = GetEpisodesToSelect(filter.EpisodeId);

            if (filter != null)
            {
                if (filter.EpisodeId != null && !filter.EpisodeId.Equals(""))
                {
                    ViewBag.Teams = GetTeamsToSelect(filter.TeamId, filter.WorkerTypeId, filter.EpisodeId);
                }
                else
                {
                    ViewBag.Teams = new List<SelectListItem>();
                }
            }
            else
            {
                ViewBag.Teams = new List<SelectListItem>();
                filter = new RankingFilterDTO();
            }

            return View(filter);
        }

        [Route("buscarResultados")]
        [HttpGet]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, string episodeId, string teamId, int workerTypeId = 0)
        {
            if (jqueryTableRequest != null)
            {

                if (Request["episodeId"] != null && !string.IsNullOrWhiteSpace(Request["episodeId"]) && !Request["episodeId"].Equals("undefined"))
                {
                    episodeId = Request["episodeId"];
                }

                if (Request["teamId"] != null && !string.IsNullOrWhiteSpace(Request["teamId"]) && !Request["teamId"].Equals("undefined"))
                {
                    teamId = Request["teamId"];
                }

                if (Request["workerTypeId"] != null && !string.IsNullOrWhiteSpace(Request["workerTypeId"]) && !Request["workerTypeId"].Equals("undefined"))
                {
                    workerTypeId = int.Parse(Request["workerTypeId"]);
                }

                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];

                List<RankingDTO> searchResult = null;

                if (episodeId != null && !episodeId.Equals(""))
                {
                    if (teamId != null && !teamId.Equals(""))
                    {
                        TeamEntity team = TeamRepository.Instance.GetByIdExterno(teamId);

                        WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(team.WorkerTypeId);

                        if (workerType.ProfileName.Equals(Profiles.LIDER))
                        {
                            List<RankingDTO> rankingDTOList = new List<RankingDTO>();

                            //List<PlayerRunEntity> playerRunList = PlayerRunRepository.Instance.GetByEpisodeAndTeam(episodeId.ToString(), teamId);
                           GetAllDTO runnersDTO = RunEngineService.Instance.GetRunsByTeamId(teamId);
                            
                            // buscar lista de run ao inves da tabela
                            foreach (RunEngineDTO item in runnersDTO.List.run)
                            {
                                RankingDTO dto = new RankingDTO();

                                WorkerDTO worker = WorkerRepository.Instance.GetDTOById(item.PlayerId);
                                //run .get player ID

                                dto = EngineBIZ.GetGlobalScoreInEpisodeByEpisodeAndPlayer(episodeId.ToString(), worker.ExternalId);
                                dto.LogoId = worker.LogoId;
                                dto.PlayerId = worker.ExternalId;
                                dto.PlayerName = worker.Name;

                                dto.LogoId = worker.LogoId;

                                rankingDTOList.Add(dto);
                            }

                            searchResult = rankingDTOList;
                        }
                        else
                        {
                            List<RankingDTO> rankingDTOList = new List<RankingDTO>();

                            GetAllDTO runnersDTO = RunEngineService.Instance.GetRunsByTeamId(teamId);
                            foreach (RunEngineDTO item in runnersDTO.List.run)

                            {
                                RankingDTO dto = new RankingDTO();

                                WorkerDTO worker = WorkerRepository.Instance.GetDTOById(item.PlayerId);
                      
                                dto = EngineBIZ.GetScoreByRun(item.Id);
                      
                                dto.PlayerName = worker.Name;
                                dto.LogoId = worker.LogoId;

                                rankingDTOList.Add(dto);
                            }

                            searchResult = rankingDTOList;
                        }
                    }
                    else
                    {
                        List<EngineTeamDTO> teamsFromEngine = new List<EngineTeamDTO>();

                        List<TeamEntity> teams = new List<TeamEntity>();

                        teamsFromEngine.AddRange(EngineBIZ.GetTeamsByEpisode(episodeId.ToString()));

                        if (workerTypeId > 0)
                        {

                            foreach (EngineTeamDTO item in teamsFromEngine)
                            {
                                TeamEntity team = new TeamEntity();
                                team = TeamRepository.Instance.GetByIdExterno(item.IdExterno);
                                teams.Add(team);
                            }

                            var teamsVar = from n in teams
                                           where n.WorkerTypeId == workerTypeId
                                           select n;

                            teams = teamsVar.ToList();
                        }
                        else
                        {
                            foreach (EngineTeamDTO item in teamsFromEngine)
                            {
                                TeamEntity team = new TeamEntity();
                                team = TeamRepository.Instance.GetByIdExterno(item.IdExterno);
                                teams.Add(team);
                            }
                        }

                        List<RankingDTO> rankingDTOList = new List<RankingDTO>();

                        foreach (TeamEntity team in teams)
                        {
                            if(team != null) { 
                                WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(team.WorkerTypeId);

                                RankingDTO rankingDTO = new RankingDTO();

                                if (workerType.ProfileName.Equals(Profiles.LIDER))
                                {
                                    WorkerEntity sponsor = WorkerRepository.Instance.GetById(team.SponsorId);

                                    rankingDTO = EngineBIZ.GetGlobalScoreInEpisodeByEpisodeAndPlayer(episodeId.ToString(), sponsor.ExternalId);
                                    rankingDTO.LogoId = team.LogoId;
                                    rankingDTO.PlayerId = team.ExternalId;
                                    rankingDTO.PlayerName = team.TeamName;
                                }
                                else
                                {
                                    rankingDTO = EngineBIZ.GetTeamScoreByTeamId(team.ExternalId);
                                    rankingDTO.LogoId = team.LogoId;
                                }

                                rankingDTOList.Add(rankingDTO);
                            }
                        }
                        searchResult = rankingDTOList;

                    }
                }
                else
                {
                    searchResult = new List<RankingDTO>();
                }

                var searchedQueryList = new List<RankingDTO>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.PlayerName != null && n.PlayerName.ToLowerInvariant().Trim().Contains(filter))
                                        orderby n.Score descending
                                        select n;

                    searchedQueryList = searchedQuery.ToList();
                }
                else
                {
                    var searchedQuery = from n in searchResult
                                        orderby n.Score descending
                                        select n;

                    searchedQueryList = searchedQuery.ToList();
                }

                int index = Int32.Parse(jqueryTableRequest.Order);

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.PlayerId != null ? r.PlayerId.ToString() : "", r.PlayerName + ";" + r.LogoId.ToString(), r.Score != null ? r.Score.Value.ToString() : 0.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.PlayerId != null ? r.PlayerId.ToString() : "", r.PlayerName + ";" + r.LogoId.ToString(), r.Score != null ? r.Score.Value.ToString() : 0.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> GetWorkerTypesToSelect(int selected)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            List<WorkerTypeEntity> workerTypes = new List<WorkerTypeEntity>();

            workerTypes.AddRange(WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.Id));

            var query = from c in workerTypes
                        select new SelectListItem
                        {
                            Text = c.TypeName,
                            Value = c.Id.ToString(),
                            Selected = c.Id == selected
                        };

            return query.ToList();
        }

        private List<SelectListItem> GetTeamsToSelect(string selected, int workerTypeId, string episodeId)
        {

            if (selected == null)
            {
                selected = "";
            }

            List<EngineTeamDTO> teamsFromEngine = new List<EngineTeamDTO>();

            List<TeamEntity> teams = new List<TeamEntity>();

            teamsFromEngine.AddRange(EngineBIZ.GetTeamsByEpisode(episodeId));

            if (workerTypeId > 0)
            {

                foreach (EngineTeamDTO item in teamsFromEngine)
                {
                    TeamEntity team = new TeamEntity();
                    team = TeamRepository.Instance.GetByIdExterno(item.IdExterno);
                    teams.Add(team);
                }

                var teamsVar = from n in teams
                               where n.WorkerTypeId == workerTypeId
                               select n;

                teams = teamsVar.ToList();
            }

            List<SelectListItem> rtn = new List<SelectListItem>();

            if (workerTypeId > 0)
            {

                var query = from c in teams
                            select new SelectListItem
                            {
                                Text = c.TeamName,
                                Value = c.ExternalId.ToString(),
                                Selected = c.ExternalId == selected.ToString()
                            };

                rtn = query.ToList();
            }
            else
            {
                var query = from c in teamsFromEngine
                            select new SelectListItem
                            {
                                Text = c.Name,
                                Value = c.IdExterno.ToString(),
                                Selected = c.IdExterno == selected.ToString()
                            };

                rtn = query.ToList();
            }

            return rtn;
        }

        private List<SelectListItem> GetEpisodesToSelect(string selected)
        {

            if (selected == null)
            {
                selected = "";
            }

            List<EpisodeEngineDTO> episodes = new List<EpisodeEngineDTO>();

            episodes.AddRange(EngineBIZ.GetEpisodesByGame(CurrentFirm.ExternalId));

            var query = from c in episodes
                        select new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id,
                            Selected = c.Id == selected
                        };

            return query.ToList();
        }

    }
}