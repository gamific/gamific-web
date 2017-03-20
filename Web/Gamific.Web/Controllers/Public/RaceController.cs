using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Controllers.Public.Model;
using Newtonsoft.Json;
using Vlast.Gamific.Model.Public.DTO;
using System.Web.Script.Serialization;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Services.Engine;

namespace Vlast.Gamific.Web.Controllers.Public
{
    [CustomAuthorize]
    [RoutePrefix("public/corrida")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,LIDER,JOGADOR")]
    public class RaceController : BaseController
    {
        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");   
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

        [Route("")]
        [HttpPost]
        public ActionResult Race(int workerTypeId = 0, int teamId = 0)
        {
            if(workerTypeId == 0)
            {
                List<WorkerTypeEntity> workerTypes = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.Id);
                ViewBag.WorkerTypes = from workerType in workerTypes
                                      select new SelectListItem
                                      {
                                          Value = workerType.Id.ToString(),
                                          Text = workerType.TypeName
                                      };

                return View("Index");
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                CarDTO dto = GetResultsFromPlayer(workerTypeId, teamId);
                int totalGoal = GoalRepository.Instance.GetTotalGoalFromWorkerType(workerTypeId);
                int numberOfMembers = 1;

                if (teamId != 0)
                {
                    ViewBag.TargetLogoId = CurrentFirm.LogoId;
                    numberOfMembers = TeamRepository.Instance.GetNumberOfMembers(teamId);
                }
                else
                {
                    ViewBag.TargetLogoId = CurrentFirm.LogoId;
                    numberOfMembers = TeamRepository.Instance.GetNumberOfTeamOfWorkerType(workerTypeId);
                }

                int totalPoints = dto.cars.Sum(p => p.Points);
                int total = totalGoal * numberOfMembers;

                
                ViewBag.TargetName = dto.TargetName;
                ViewBag.TotalGoal = total;
                ViewBag.TotalReached = numberOfMembers == 0 ? 0 : (Math.Round((totalPoints * 100) / (float)total));
                ViewBag.Result = serializer.Serialize(dto);

                return View("Race");
            }

        }
        
        [Route("buscarScore")]
        [HttpGet]
        public ActionResult GetScore(string episodeId, string teamId, string metricId = "0" )
        {
            CarDTO dto = new CarDTO();

            if(teamId != null && teamId != "")
            {
                dto = GetResultsFromTeam(teamId, metricId);
            }
            else if(episodeId != null && episodeId != "")
            {
                dto = GetResultsFromEpisode(episodeId, metricId);
            }

            return Json(JsonConvert.SerializeObject(dto), JsonRequestBehavior.AllowGet);
        }


        private CarDTO GetResultsFromEpisode(string episodeId, string metricId)
        {
            if (episodeId != null && episodeId != "")
            {
                CarDTO dto = new CarDTO();

                string logoPath = CurrentURL + CurrentFirm.LogoId;

                dto.cars = new List<WorkerCarDTO>();
                dto.CompanyLogo = CurrentFirm.LogoId;
                dto.Target = Target3D.PLAYER;
                dto.LogoPathOutdoor1 = logoPath;
                dto.LogoPathOutdoor2 = logoPath;
                dto.LogoPathOutdoor3 = logoPath;
                dto.LogoPathOutdoor4 = logoPath;
                dto.LogoPathBanner1 = logoPath;
                dto.LogoPathBanner2 = logoPath;

                GetAllDTO all = TeamEngineService.Instance.GetAllTeamScoreByEpisodeId(episodeId, metricId);

                int i = 1;
                dto.cars = new List<WorkerCarDTO>();
                foreach(TeamEngineDTO team in all.List.team)
                {

                    dto.cars.Add(new WorkerCarDTO{
                        Name = team.Nick,
                        CarColor1 = GenerateColorHexadecimal(1 * i),
                        CarColor2 = GenerateColorHexadecimal(2 * i),
                        HelmetColor = GenerateColorHexadecimal(3 * i),
                        LogoId = team.LogoId,
                        Points = (int)team.Score,
                        AvatarPath = CurrentURL + team.LogoId,
                        CarLogo = logoPath
                    });
                    i++;
                }

                return dto;
            }

            return null;
        }

        private CarDTO GetResultsFromTeam(string teamId, string metricId)
        {
            if(teamId != null && teamId != "")
            {
                CarDTO dto = new CarDTO();

                string logoPath = CurrentURL + CurrentFirm.LogoId;

                dto.cars = new List<WorkerCarDTO>();
                dto.CompanyLogo = CurrentFirm.LogoId;
                dto.Target = Target3D.PLAYER;
                dto.LogoPathOutdoor1 = logoPath;
                dto.LogoPathOutdoor2 = logoPath;
                dto.LogoPathOutdoor3 = logoPath;
                dto.LogoPathOutdoor4 = logoPath;
                dto.LogoPathBanner1 = logoPath;
                dto.LogoPathBanner2 = logoPath;

                GetAllDTO all = RunEngineService.Instance.GetAllRunScore(teamId, metricId);
                List<PlayerEngineDTO> players = (from run in all.List.run select PlayerEngineService.Instance.GetById(run.PlayerId)).ToList();

                all.List.result = (from run in all.List.run
                                   from player in players
                                   where run.PlayerId == player.Id
                                   select new ResultEngineDTO
                                   {
                                       Nick = player.Nick,
                                       Score = run.Score,
                                       LogoId = player.LogoId
                                   }).ToList();

                dto.cars = (from run in all.List.run
                            from player in players
                            where run.PlayerId == player.Id
                            select new WorkerCarDTO
                            {
                                Name = player.Nick,
                                CarColor1 = GenerateColorHexadecimal(1),
                                CarColor2 = GenerateColorHexadecimal(2),
                                HelmetColor = GenerateColorHexadecimal(3),
                                LogoId = player.LogoId,
                                Points = (int)run.Score,
                                AvatarPath = CurrentURL + player.LogoId,
                                CarLogo = logoPath
                            }).ToList();

                return dto;
            }

            return null;
        }

        /// <summary>
        /// Cria a lista de resultados do jogador
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private CarDTO GetResultsFromPlayer(int workerTypeId = 0, int teamId = 0)
        {
            CarDTO dto = new CarDTO();
            dto.cars = new List<WorkerCarDTO>();
            dto.CompanyLogo = CurrentFirm.LogoId;
            dto.Target = Target3D.PLAYER;

            string logoPath = CurrentURL + CurrentFirm.LogoId;

            dto.LogoPathOutdoor1 = logoPath;
            dto.LogoPathOutdoor2 = logoPath;
            dto.LogoPathOutdoor3 = logoPath;
            dto.LogoPathOutdoor4 = logoPath;
            dto.LogoPathBanner1 = logoPath;
            dto.LogoPathBanner2 = logoPath;
            

            if (CurrentWorkerType.ProfileName == Profiles.JOGADOR)
            {
                TeamEntity team = TeamRepository.Instance.GetFromWorker(CurrentWorker.Id);

                List<WorkerDTO> workers = WorkerRepository.Instance.GetAllFromTeam(team.Id);

                foreach (WorkerDTO worker in workers)
                {
                    List<ResultMetricDTO> results = new List<ResultMetricDTO>();//ResultRepository.Instance.GetAllFromWorkerByPeriod(worker.IdWorker, new DateTime(), DateTime.Now.AddYears(1000));

                    int totalPoints = 1;

                    foreach (ResultMetricDTO result in results)
                    {
                        totalPoints += result.Points;
                    }

                    WorkerCarDTO car = new WorkerCarDTO
                    {
                        Name = worker.Name,
                        IdTarget = worker.IdWorker,
                        CarColor1 = GenerateColorHexadecimal(1),
                        CarColor2 = GenerateColorHexadecimal(2),
                        HelmetColor = GenerateColorHexadecimal(3),
                        LogoId = worker.LogoId,
                        Points = totalPoints,
                        AvatarPath = CurrentURL + worker.LogoId,
                        CarLogo = logoPath
                    };

                    if (worker.IdWorker == CurrentWorker.Id)
                    {
                        car.isFirstPerson = true;
                    }
                    else
                    {
                        car.isFirstPerson = false;
                    }

                    dto.cars.Add(car);
                }
            }
            else if (teamId != 0)
            {
                List<WorkerDTO> workers = WorkerRepository.Instance.GetAllFromTeam(teamId);

                foreach (WorkerDTO worker in workers)
                {
                    List<ResultMetricDTO> results = new List<ResultMetricDTO>();//ResultRepository.Instance.GetAllFromWorkerByPeriod(worker.IdWorker, new DateTime(), DateTime.Now.AddYears(1000));

                    int totalPoints = 1;

                    foreach (ResultMetricDTO result in results)
                    {
                        totalPoints += result.Points;
                    }

                    WorkerCarDTO car = new WorkerCarDTO
                    {
                        Name = worker.Name,
                        IdTarget = worker.IdWorker,
                        CarColor1 = GenerateColorHexadecimal(1),
                        CarColor2 = GenerateColorHexadecimal(2),
                        HelmetColor = GenerateColorHexadecimal(3),
                        LogoId = worker.LogoId,
                        Points = totalPoints,
                        AvatarPath = CurrentURL + worker.LogoId
                    };

                    car.isFirstPerson = false;

                    dto.cars.Add(car);
                }
            }

            return dto;
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


        /**
        /// <summary>
        /// Filtro para a tela de corrida
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        [Route("")]
        public ActionResult Index(FilterResultDTO filter, int? destination)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            WorkerEntity worker = WorkerRepository.Instance.GetByUserId(CurrentUserId);

            WorkerDTO workerDTO = WorkerRepository.Instance.GetDTOById(worker.Id, CurrentFirm.Id);

            WorkerTypeEntity profile = WorkerTypeRepository.Instance.GetById(worker.ProfileId);

            bool flagEnableCampaigns = false;
            bool flagEnableTeamsFromCampaign = false;
            bool flagEnablePlayersFromTeam = false;
            List<WorkerDTO> workersToSelect = new List<WorkerDTO>();
            List<TeamDTO> teamsToSelect = new List<TeamDTO>();
            List<CampaignEntity> campaignsToSelect = new List<CampaignEntity>();

            if (profile.ProfileName.Equals("ADMINISTRADOR"))
            {
                flagEnableCampaigns = true;
                flagEnableTeamsFromCampaign = true;
                flagEnablePlayersFromTeam = true;

                if (filter.CampaignId > 0)
                {
                    if (filter.TeamId > 0)
                    {
                        if (filter.PlayerId > 0)
                        {

                            workersToSelect = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, filter.TeamId, 0, 1000);
                            teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                            campaignsToSelect = CampaignRepository.Instance.GetAllFromFirm(CurrentFirm.Id, 0, 1000);

                            CarDTO dto = GetResultsFromPlayer(filter.CampaignId, filter.TeamId, filter.PlayerId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                        else
                        {
                            workersToSelect = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, filter.TeamId, 0, 1000);
                            teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                            campaignsToSelect = CampaignRepository.Instance.GetAllFromFirm(CurrentFirm.Id, 0, 1000);

                            CarDTO dto = GetResultsFromTeam(filter.CampaignId, filter.TeamId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                    }
                    else
                    {
                        flagEnablePlayersFromTeam = false;
                        teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                        campaignsToSelect = CampaignRepository.Instance.GetAllFromFirm(CurrentFirm.Id, 0, 1000);

                        CarDTO dto = GetResultsFromCampaign(filter.CampaignId);

                        ViewBag.TargetName = dto.TargetName;
                        ViewBag.TargetLogoId = dto.TargetLogoId;
                        ViewBag.TotalGoal = dto.TotalGoal;
                        ViewBag.TotalReached = dto.PercentFromGoalReached;
                        ViewBag.Result = serializer.Serialize(dto);
                    }
                }
                else
                {
                    flagEnablePlayersFromTeam = false;
                    flagEnableTeamsFromCampaign = false;
                    campaignsToSelect = CampaignRepository.Instance.GetAllFromFirm(CurrentFirm.Id, 0, 1000);
                }

            }

            if (profile.ProfileName.Equals("SUPERVISOR DE CAMPANHA"))
            {
                flagEnableCampaigns = true;
                flagEnableTeamsFromCampaign = true;
                flagEnablePlayersFromTeam = true;

                if (filter.CampaignId > 0)
                {
                    if (filter.TeamId > 0)
                    {
                        if (filter.PlayerId > 0)
                        {
                            campaignsToSelect = CampaignRepository.Instance.GetBySponsor(worker.Id);

                            List<CampaignEntity> campaignsPlaying = new List<CampaignEntity>();

                            List<TeamEntity> teams = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                            TeamEntity teamSelected = TeamRepository.Instance.GetById(filter.TeamId);

                            if (teams.Contains(teamSelected))
                            {
                                workersToSelect = new List<WorkerDTO>();
                                workersToSelect.Add(workerDTO);
                            }
                            else
                            {
                                workersToSelect = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, filter.TeamId, 0, 1000);
                            }

                            foreach (TeamEntity item in teams)
                            {

                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsPlaying.AddRange(campaignsToAdd);
                                }

                            }

                            CampaignEntity campaignSelected = CampaignRepository.Instance.GetById(filter.CampaignId);

                            if (campaignsToSelect.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                            }
                            else if (campaignsPlaying.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                            }

                            campaignsToSelect.AddRange(campaignsPlaying);

                            CarDTO dto = GetResultsFromPlayer(filter.CampaignId, filter.TeamId, filter.PlayerId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                        else
                        {
                            campaignsToSelect = CampaignRepository.Instance.GetBySponsor(worker.Id);

                            List<CampaignEntity> campaignsPlaying = new List<CampaignEntity>();

                            List<TeamEntity> teams = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                            TeamEntity teamSelected = TeamRepository.Instance.GetById(filter.TeamId);

                            if (teams.Contains(teamSelected))
                            {
                                workersToSelect = new List<WorkerDTO>();
                                workersToSelect.Add(workerDTO);
                            }
                            else
                            {
                                workersToSelect = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, filter.TeamId, 0, 1000);
                            }

                            foreach (TeamEntity item in teams)
                            {

                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsPlaying.AddRange(campaignsToAdd);
                                }
                            }

                            CampaignEntity campaignSelected = CampaignRepository.Instance.GetById(filter.CampaignId);

                            if (campaignsToSelect.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                            }
                            else if (campaignsPlaying.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                            }

                            campaignsToSelect.AddRange(campaignsPlaying);

                            CarDTO dto = GetResultsFromTeam(filter.CampaignId, filter.TeamId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                    }
                    else
                    {
                        flagEnablePlayersFromTeam = false;

                        campaignsToSelect = CampaignRepository.Instance.GetBySponsor(worker.Id);

                        List<TeamEntity> teams = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                        List<CampaignEntity> campaignsPlaying = new List<CampaignEntity>();

                        foreach (TeamEntity item in teams)
                        {

                            List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                            if (campaignsToAdd != null)
                            {
                                campaignsPlaying.AddRange(campaignsToAdd);
                            }
                        }

                        CampaignEntity campaignSelected = CampaignRepository.Instance.GetById(filter.CampaignId);

                        if (campaignsToSelect.Contains(campaignSelected))
                        {
                            teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                        }
                        else if (campaignsPlaying.Contains(campaignSelected))
                        {
                            teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                        }

                        campaignsToSelect.AddRange(campaignsPlaying);

                        CarDTO dto = GetResultsFromCampaign(filter.CampaignId);

                        ViewBag.TargetName = dto.TargetName;
                        ViewBag.TargetLogoId = dto.TargetLogoId;
                        ViewBag.TotalGoal = dto.TotalGoal;
                        ViewBag.TotalReached = dto.PercentFromGoalReached;
                        ViewBag.Result = serializer.Serialize(dto);
                    }
                }
                else
                {
                    flagEnableTeamsFromCampaign = false;
                    flagEnablePlayersFromTeam = false;
                    campaignsToSelect = CampaignRepository.Instance.GetBySponsor(worker.Id);
                    List<TeamEntity> teams = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                    foreach (TeamEntity item in teams)
                    {

                        List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                        if (campaignsToAdd != null)
                        {
                            campaignsToSelect.AddRange(campaignsToAdd);
                        }

                    }
                }

            }

            if (profile.ProfileName.Equals("SUPERVISOR DE EQUIPE"))
            {
                flagEnableCampaigns = true;
                flagEnableTeamsFromCampaign = true;
                flagEnablePlayersFromTeam = true;

                if (filter.CampaignId > 0)
                {
                    if (filter.TeamId > 0)
                    {
                        if (filter.PlayerId > 0)
                        {
                            TeamEntity teamSelected = TeamRepository.Instance.GetById(filter.TeamId);

                            List<CampaignEntity> campaignsPlaying = new List<CampaignEntity>();

                            List<TeamEntity> teamsSponsor = TeamRepository.Instance.GetBySponsor(worker.Id);

                            foreach (TeamEntity item in teamsSponsor)
                            {
                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsToSelect.AddRange(campaignsToAdd);
                                }
                            }

                            List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                            foreach (TeamEntity item in teamsPlaying)
                            {

                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsPlaying.AddRange(campaignsToAdd);
                                }

                            }

                            if (teamsSponsor.Contains(teamSelected))
                            {
                                workersToSelect = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, filter.TeamId, 0, 1000);
                            }
                            else
                            {
                                workersToSelect = new List<WorkerDTO>();
                                workersToSelect.Add(workerDTO);
                            }

                            CampaignEntity campaignSelected = CampaignRepository.Instance.GetById(filter.CampaignId);

                            if (campaignsToSelect.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                            }
                            else if (campaignsPlaying.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                            }

                            campaignsToSelect.AddRange(campaignsPlaying);

                            CarDTO dto = GetResultsFromPlayer(filter.CampaignId, filter.TeamId, filter.PlayerId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                        else
                        {
                            List<TeamEntity> teamsSponsor = TeamRepository.Instance.GetBySponsor(worker.Id);

                            List<CampaignEntity> campaignsPlaying = new List<CampaignEntity>();

                            foreach (TeamEntity item in teamsSponsor)
                            {

                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsToSelect.AddRange(campaignsToAdd);
                                }

                            }

                            List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                            foreach (TeamEntity item in teamsPlaying)
                            {

                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsPlaying.AddRange(campaignsToAdd);
                                }

                            }

                            TeamEntity teamSelected = TeamRepository.Instance.GetById(filter.TeamId);

                            if (teamsSponsor.Contains(teamSelected))
                            {
                                workersToSelect = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, filter.TeamId, 0, 1000);
                            }
                            else
                            {
                                workersToSelect = new List<WorkerDTO>();
                                workersToSelect.Add(workerDTO);
                            }

                            CampaignEntity campaignSelected = CampaignRepository.Instance.GetById(filter.CampaignId);

                            if (campaignsToSelect.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                            }
                            else if (campaignsPlaying.Contains(campaignSelected))
                            {
                                teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                            }

                            campaignsToSelect.AddRange(campaignsPlaying);

                            CarDTO dto = GetResultsFromTeam(filter.CampaignId, filter.TeamId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                    }
                    else
                    {
                        flagEnablePlayersFromTeam = false;
                        List<TeamEntity> teamsSponsor = TeamRepository.Instance.GetBySponsor(worker.Id);

                        List<CampaignEntity> campaignsPlaying = new List<CampaignEntity>();

                        foreach (TeamEntity item in teamsSponsor)
                        {

                            List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                            if (campaignsToAdd != null)
                            {
                                campaignsToSelect.AddRange(campaignsToAdd);
                            }

                        }

                        List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                        foreach (TeamEntity item in teamsPlaying)
                        {

                            List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                            if (campaignsToAdd != null)
                            {
                                campaignsPlaying.AddRange(campaignsToAdd);
                            }

                        }

                        CampaignEntity campaignSelected = CampaignRepository.Instance.GetById(filter.CampaignId);

                        if (campaignsToSelect.Contains(campaignSelected))
                        {
                            teamsToSelect = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, filter.CampaignId, 0, 1000);
                        }
                        else if (campaignsPlaying.Contains(campaignSelected))
                        {
                            teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                        }

                        campaignsToSelect.AddRange(campaignsPlaying);

                        CarDTO dto = GetResultsFromCampaign(filter.CampaignId);

                        ViewBag.TargetName = dto.TargetName;
                        ViewBag.TargetLogoId = dto.TargetLogoId;
                        ViewBag.TotalGoal = dto.TotalGoal;
                        ViewBag.TotalReached = dto.PercentFromGoalReached;
                        ViewBag.Result = serializer.Serialize(dto);
                    }
                }
                else
                {
                    flagEnableTeamsFromCampaign = false;
                    flagEnablePlayersFromTeam = false;

                    List<TeamEntity> teamsSponsor = TeamRepository.Instance.GetBySponsor(worker.Id);

                    foreach (TeamEntity item in teamsSponsor)
                    {

                        List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                        if (campaignsToAdd != null)
                        {
                            campaignsToSelect.AddRange(campaignsToAdd);
                        }

                    }

                    List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                    foreach (TeamEntity item in teamsPlaying)
                    {

                        List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                        if (campaignsToAdd != null)
                        {
                            campaignsToSelect.AddRange(campaignsToAdd);
                        }

                    }
                }
            }

            if (profile.ProfileName.Equals("JOGADOR"))
            {
                flagEnableCampaigns = true;
                flagEnableTeamsFromCampaign = true;
                flagEnablePlayersFromTeam = true;

                if (filter.CampaignId > 0)
                {
                    if (filter.TeamId > 0)
                    {
                        if (filter.PlayerId > 0)
                        {
                            workersToSelect = new List<WorkerDTO>();
                            workersToSelect.Add(workerDTO);
                            teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                            List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                            foreach (TeamEntity item in teamsPlaying)
                            {

                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsToSelect.AddRange(campaignsToAdd);
                                }

                            }

                            CarDTO dto = GetResultsFromPlayer(filter.CampaignId, filter.TeamId, filter.PlayerId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                        else
                        {
                            workersToSelect = new List<WorkerDTO>();
                            workersToSelect.Add(workerDTO);
                            teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);
                            List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                            foreach (TeamEntity item in teamsPlaying)
                            {

                                List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                                if (campaignsToAdd != null)
                                {
                                    campaignsToSelect.AddRange(campaignsToAdd);
                                }

                            }

                            CarDTO dto = GetResultsFromTeam(filter.CampaignId, filter.TeamId);

                            ViewBag.TargetName = dto.TargetName;
                            ViewBag.TargetLogoId = dto.TargetLogoId;
                            ViewBag.TotalGoal = dto.TotalGoal;
                            ViewBag.TotalReached = dto.PercentFromGoalReached;
                            ViewBag.Result = serializer.Serialize(dto);
                        }
                    }
                    else
                    {
                        flagEnablePlayersFromTeam = false;
                        List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                        foreach (TeamEntity item in teamsPlaying)
                        {

                            List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                            if (campaignsToAdd != null)
                            {
                                campaignsToSelect.AddRange(campaignsToAdd);
                            }

                        }
                        teamsToSelect = TeamRepository.Instance.GetDTOByPlayerAndCampaign(worker.Id, filter.CampaignId);

                        CarDTO dto = GetResultsFromCampaign(filter.CampaignId);

                        ViewBag.TargetName = dto.TargetName;
                        ViewBag.TargetLogoId = dto.TargetLogoId;
                        ViewBag.TotalGoal = dto.TotalGoal;
                        ViewBag.TotalReached = dto.PercentFromGoalReached;
                        ViewBag.Result = serializer.Serialize(dto);
                    }
                }
                else
                {
                    flagEnableTeamsFromCampaign = false;
                    flagEnablePlayersFromTeam = false;
                    List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                    foreach (TeamEntity item in teamsPlaying)
                    {

                        List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                        if (campaignsToAdd != null)
                        {
                            campaignsToSelect.AddRange(campaignsToAdd);
                        }

                    }
                }
            }

            ViewBag.FlagEnableCampaigns = flagEnableCampaigns;
            ViewBag.FlagEnableTeamsFromCampaign = flagEnableTeamsFromCampaign;
            ViewBag.FlagEnablePlayersFromTeam = flagEnablePlayersFromTeam;

            if (campaignsToSelect != null)
            {
                ViewBag.Campaigns = GetCampaignsToSelect(filter.CampaignId, campaignsToSelect);
            }
            if (teamsToSelect != null)
            {
                ViewBag.Teams = GetTeamsToSelect(filter.TeamId, teamsToSelect);
            }
            if (workersToSelect != null)
            {
                ViewBag.Players = GetPlayersToSelect(filter.PlayerId, workersToSelect);
            }

            if (destination == 1)
            {
                ViewBag.Filter = filter;

                return View("Race");
            }
            else
            {
                return View(filter);
            }

        }

        /// <summary>
        /// Cria a lista de seleção das campanhas
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="campaigns"></param>
        /// <returns></returns>
        private List<SelectListItem> GetCampaignsToSelect(int selected, List<CampaignEntity> campaigns)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            var query = from c in campaigns
                        select new SelectListItem
                        {
                            Text = c.CampaignName,
                            Value = c.Id.ToString(),
                            Selected = c.Id == selected
                        };

            return query.ToList();
        }

        /// <summary>
        /// Cria a lista de seleção das equipes
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="teams"></param>
        /// <returns></returns>
        private List<SelectListItem> GetTeamsToSelect(int selected, List<TeamDTO> teams)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            var query = from c in teams
                        select new SelectListItem
                        {
                            Text = c.TeamName,
                            Value = c.IdTeam.ToString(),
                            Selected = c.IdTeam == selected
                        };

            return query.ToList();
        }

        /// <summary>
        /// Cria a lista de seleção dos jogadores
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="workers"></param>
        /// <returns></returns>
        private List<SelectListItem> GetPlayersToSelect(int selected, List<WorkerDTO> workers)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            var query = from c in workers
                        select new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.IdWorker.ToString(),
                            Selected = c.IdWorker == selected
                        };

            return query.ToList();
        }

        /// <summary>
        /// Cria a lista de resultados do jogador
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="teamId"></param>
        /// <param name="workerId"></param>
        /// <returns></returns>
        private CarDTO GetResultsFromPlayer(int campaignId, int teamId, int workerId)
        {
            List<MetricEntity> list = new List<MetricEntity>();

            list = MetricRepository.Instance.GetAllFromCampaignAndTeam(CurrentFirm.Id, teamId, campaignId);

            List<WorkerDTO> workers = new List<WorkerDTO>();

            WorkerDTO thisWorker = new WorkerDTO();

            thisWorker.IdWorker = workerId;

            workers = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, teamId, 0, 20);

            TeamGoalEntity goal = TeamGoalRepository.Instance.GetFromTeamAndCampaign(CurrentFirm.Id, teamId, campaignId);

            CarDTO dto = new CarDTO();

            WorkerDTO workerSelected = WorkerRepository.Instance.GetDTOById(workerId, CurrentFirm.Id);

            if (!workers.Contains(thisWorker))
            {
                workers.Remove(workers.Last());
                workers.Add(workerSelected);
            }

            dto.cars = new List<WorkerCarDTO>();
            dto.CompanyLogo = CurrentFirm.LogoId;
            dto.LogoPath = ParameterCache.Get("S3_URL") + CurrentFirm.LogoId;
            dto.Target = Target3D.PLAYER;
            dto.TargetName = workerSelected.Name;
            dto.TargetLogoId = workerSelected.LogoId;

            foreach (WorkerDTO worker in workers)
            {
                WorkerCarDTO carDTO = new WorkerCarDTO();

                int pointsValueTotal = 0;
                int totalResult = 0;

                foreach (MetricEntity item in list)
                {

                    MetricEntity metric = new MetricEntity();

                    int? result = ResultRepository.Instance.GetAllFromPlayerAndMetricAndTeamAndCampaign(CurrentFirm.Id, teamId, campaignId, worker.IdWorker, item.Id);

                    if (result != null)
                    {
                        totalResult += result.Value;
                    }

                    metric = ConvertResultInPoints(item, result);

                    pointsValueTotal += metric.PointsValue;

                }

                carDTO.Name = worker.Name;
                carDTO.IdTarget = worker.IdWorker;
                carDTO.CarColor1 = GenerateColorHexadecimal(1);
                carDTO.CarColor2 = GenerateColorHexadecimal(2);
                carDTO.HelmetColor = GenerateColorHexadecimal(3);
                carDTO.LogoId = worker.LogoId;
                carDTO.Points = pointsValueTotal;
                carDTO.AvatarPath = ParameterCache.Get("S3_URL") + worker.LogoId;

                if (workerId == worker.IdWorker)
                {
                    carDTO.isFirstPerson = true;

                    dto.TotalGoal = goal.Goal / workers.Count();

                    if (totalResult == 0)
                    {
                        totalResult = 1;
                    }

                    dto.PercentFromGoalReached = 100 * totalResult / (goal.Goal / workers.Count());
                }
                else
                {
                    carDTO.isFirstPerson = false;
                }

                dto.cars.Add(carDTO);
            }

            return dto;
        }

        /// <summary>
        /// Cria a lista de resultados da equipe
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private CarDTO GetResultsFromTeam(int campaignId, int teamId)
        {
            List<MetricEntity> list = new List<MetricEntity>();

            list = MetricRepository.Instance.GetAllFromCampaignAndTeam(CurrentFirm.Id, teamId, campaignId);

            List<WorkerDTO> workers = new List<WorkerDTO>();

            workers = WorkerRepository.Instance.GetAllFromTeam(CurrentFirm.Id, teamId, 0, 20);

            TeamGoalEntity goal = TeamGoalRepository.Instance.GetFromTeamAndCampaign(CurrentFirm.Id, teamId, campaignId);

            int totalResult = 0;

            CarDTO dto = new CarDTO();

            TeamEntity team = TeamRepository.Instance.GetById(teamId);

            dto.cars = new List<WorkerCarDTO>();
            dto.CompanyLogo = CurrentFirm.LogoId;
            dto.LogoPath = ParameterCache.Get("S3_URL") + CurrentFirm.LogoId;
            dto.Target = Target3D.TEAM;
            dto.TargetName = team.TeamName;
            dto.TargetLogoId = team.LogoId;

            foreach (WorkerDTO worker in workers)
            {

                WorkerCarDTO carDTO = new WorkerCarDTO();

                int totalPoints = 0;

                foreach (MetricEntity item in list)
                {
                    MetricEntity metric = new MetricEntity();

                    int? result = ResultRepository.Instance.GetAllFromPlayerAndMetricAndTeamAndCampaign(CurrentFirm.Id, teamId, campaignId, worker.IdWorker, item.Id);

                    if (result != null)
                    {
                        totalResult += result.Value;
                    }

                    metric = ConvertResultInPoints(item, result);

                    totalPoints += metric.PointsValue;
                }

                carDTO.Name = worker.Name;
                carDTO.IdTarget = worker.IdWorker;
                carDTO.CarColor1 = GenerateColorHexadecimal(1);
                carDTO.CarColor2 = GenerateColorHexadecimal(2);
                carDTO.HelmetColor = GenerateColorHexadecimal(3);
                carDTO.LogoId = worker.LogoId;
                carDTO.Points = totalPoints;
                carDTO.isFirstPerson = false;
                carDTO.AvatarPath = ParameterCache.Get("S3_URL") + worker.LogoId;

                dto.cars.Add(carDTO);
            }

            dto.TotalGoal = goal.Goal;

            if (totalResult == 0)
            {
                totalResult = 1;
            }

            dto.PercentFromGoalReached = 100 * totalResult / (goal.Goal);

            return dto;
        }

        /// <summary>
        /// Cria a lista de resultados da campanha
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        private CarDTO GetResultsFromCampaign(int campaignId)
        {
            List<MetricEntity> list = new List<MetricEntity>();

            list = MetricRepository.Instance.GetAllFromCampaign(campaignId);

            List<TeamDTO> teams = new List<TeamDTO>();

            teams = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, campaignId, 0, 20);

            List<TeamGoalEntity> goals = TeamGoalRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, campaignId);

            int totalGoal = 0;
            int totalResult = 0;

            foreach (TeamGoalEntity goal in goals)
            {
                totalGoal += goal.Goal;
            }

            CarDTO dto = new CarDTO();

            CampaignEntity campaign = CampaignRepository.Instance.GetById(campaignId);

            dto.cars = new List<WorkerCarDTO>();
            dto.CompanyLogo = CurrentFirm.LogoId;
            dto.Target = Target3D.TEAM;
            dto.LogoPath = ParameterCache.Get("S3_URL") + CurrentFirm.LogoId;
            dto.TargetName = campaign.CampaignName;
            dto.TargetLogoId = CurrentFirm.LogoId;

            foreach (TeamDTO team in teams)
            {
                WorkerCarDTO carDTO = new WorkerCarDTO();

                int totalPoints = 0;

                foreach (MetricEntity item in list)
                {
                    MetricEntity metric = new MetricEntity();

                    int? result = ResultRepository.Instance.GetAllFromTeamAndMetricAndCampaign(CurrentFirm.Id, team.IdTeam, campaignId, item.Id);

                    if (result != null)
                    {
                        totalResult += result.Value;
                    }

                    metric = ConvertResultInPoints(item, result);

                    totalPoints += metric.PointsValue;
                }

                carDTO.Name = team.TeamName;
                carDTO.IdTarget = team.IdTeam;
                carDTO.CarColor1 = GenerateColorHexadecimal(1);
                carDTO.CarColor2 = GenerateColorHexadecimal(2);
                carDTO.HelmetColor = GenerateColorHexadecimal(3);
                carDTO.LogoId = team.LogoId;
                carDTO.Points = totalPoints;
                carDTO.AvatarPath = ParameterCache.Get("S3_URL") + team.LogoId;
                carDTO.isFirstPerson = false;

                dto.cars.Add(carDTO);
            }

            dto.TotalGoal = totalGoal;

            if (totalResult == 0)
            {
                totalResult = 1;
            }

            dto.PercentFromGoalReached = 100 * totalResult / (totalGoal);

            return dto;
        }
    **/
    }
}