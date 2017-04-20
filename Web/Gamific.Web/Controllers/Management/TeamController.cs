using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Media.Domain;
using Vlast.Gamific.Model.Media.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    [RoutePrefix("admin/equipes")]
    public class TeamController : BaseController
    {
        #region Equipes

        // GET: Team
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Abre o modal para edição de uma equipe
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Route("editar/{teamId}")]
        public ActionResult Edit(string teamId)
        {
            TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);

            ViewBag.Episodes = GetEpisodesToSelect(team.EpisodeId);
            ViewBag.Sponsors = (IEnumerable<SelectListItem>)GetSponsorsToSelect(team.MasterPlayerId);
            ViewBag.SponsorId = team.MasterPlayerId;
            ViewBag.EpisodeId = team.EpisodeId;

            return PartialView("_Edit", team);
        }

        /// <summary>
        /// Abre o modal para cadastro de uma nova equipe
        /// </summary>
        /// <returns></returns>
        [Route("cadastrar")]
        public ActionResult Create()
        {
            TeamEngineDTO team = new TeamEngineDTO();

            ViewBag.Sponsors = GetSponsorsToSelect();
            ViewBag.Episodes = GetEpisodesToSelect();

            return PartialView("_Edit", team);
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
        /// Remove uma equipe
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Route("remover/{teamId}")]
        public ActionResult Remove(string teamId)
        {
            TeamEngineService.Instance.RemoveTeamFromEpisode(teamId);

            return new EmptyResult();
        }

        /// <summary>
        /// Salva as informações de uma equipe quando editada ou criada
        /// </summary>
        /// <param name="team"></param>
        /// <param name="logoUpload"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(TeamEngineDTO team, HttpPostedFileBase logoUpload)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    ImageEntity imageSaving = new ImageEntity();
                    if (logoUpload != null && logoUpload.ContentLength > 0)
                    {
                        imageSaving.Status = GenericStatus.ACTIVE;
                        imageSaving.UpdatedBy = CurrentUserId;

                        byte[] cover = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            logoUpload.InputStream.CopyTo(memoryStream);
                            if (memoryStream.Length > 0)
                            {
                                using (Image image = Bitmap.FromStream(memoryStream))
                                {
                                    logoUpload.InputStream.CopyTo(memoryStream);
                                    if (memoryStream.Length > 0)
                                    {
                                        cover = memoryStream.ToArray();
                                    }
                                }
                            }
                        }

                        //if (team.LogoId > 0)
                        {
                            //imageSaving.Id = team.LogoId;
                        }
                        //else
                        {
                            imageSaving = ImageRepository.Instance.CreateImage(imageSaving);
                        }

                        ImageRepository.Instance.SaveOrReplaceLogo(imageSaving.Id, cover);

                        team.LogoId = imageSaving.Id;
                    }

                    if (team.GameId == null)
                    {
                        team.GameId = CurrentFirm.ExternalId;
                    }
                    
                    ValidateModel(team);

                    if (team.Id != null)
                    {
                        TeamEngineDTO teamTemp = TeamEngineService.Instance.UpdateTeamMaster(team.MasterPlayerId, team.Id);
                        teamTemp.LogoId = logoUpload != null ? imageSaving.Id : teamTemp.LogoId;
                        teamTemp.LogoPath = CurrentURL + teamTemp.LogoId;
                        teamTemp.Nick = team.Nick; 
                        team = TeamEngineService.Instance.CreateOrUpdate(teamTemp);
                    }
                    else
                    {
                        team.LogoPath = CurrentURL + team.LogoId;
                        team = TeamEngineService.Instance.CreateOrUpdate(team);
                        List<PlayerEngineDTO> listPlayers = new List<PlayerEngineDTO>();
                        listPlayers.Add(PlayerEngineService.Instance.GetById(team.MasterPlayerId));
                        List<RunEngineDTO> runs = TeamEngineService.Instance.JoinPlayersOnTeam(team.Id, listPlayers);
                    }

                    scope.Complete();

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Exception = e.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Carrega um datatable com todas as equipes da firma
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <returns></returns>
        [Route("search/{episodeId}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, string episodeId)
        {
            if (jqueryTableRequest != null)
            {

                List<string> externalIds = new List<string>();

                GetAllDTO all = TeamEngineService.Instance.FindByEpisodeIdAndGameId(episodeId, CurrentFirm.ExternalId, jqueryTableRequest.Page);

                externalIds.AddRange((from team in all.List.team where team.MasterPlayerId != null select team.MasterPlayerId).ToList());

                List<WorkerDTO> sponsors = WorkerRepository.Instance.GetWorkerDTOByListExternalId(externalIds);

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
                        Data = all.List.team.Select(t => new string[] { t.Nick + ";" + t.LogoId, (t.MasterPlayerId == null ? "Não há responsável por essa equipe." : sponsors.Find(w => w.ExternalId == t.MasterPlayerId) != null ? sponsors.Find(w => w.ExternalId == t.MasterPlayerId).Name : ""), t.Id }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.team.Select(t => new string[] { t.Nick + ";" + t.LogoId, (t.MasterPlayerId == null ? "Não há responsável por essa equipe." : sponsors.Find(w => w.ExternalId == t.MasterPlayerId) != null ? sponsors.Find(w => w.ExternalId == t.MasterPlayerId).Name : ""), t.Id }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Detalhes de equipe

        /// <summary>
        /// Abre a tela de detalhes de uma equipe
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Route("detalheEquipe/{teamId}")]
        public ActionResult ViewTeam(string teamId)
        {
            TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);

            RefreshWorkersOnTeamCache(teamId);

            ViewBag.CurrentTeam = teamId;
            ViewBag.TeamName = team.Nick;

            return View();
        }

        /// <summary>
        /// abre o modal para associar equipes a jogadores
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Route("associarJogadores/{teamId}")]
        public ActionResult AssociatePlayers(string teamId)
        {
            ViewBag.CurrentTeam = teamId;

            List<WorkerTypeEntity> workerTypes = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.ExternalId);

            ViewBag.WorkerTypes = from workerType in workerTypes
                                  select new SelectListItem
                                  {
                                      Text = workerType.TypeName,
                                      Value = workerType.Id.ToString()
                                  };

            GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId);

            List<string> workerExternalIds = (from run in all.List.run select run.PlayerId).ToList();
            


            return PartialView("_AssociatePlayers");
        }

        /// <summary>
        /// Remove a associação de jogador com equipe
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Route("removerAssociacao/{playerId}/{teamId}")]
        public ActionResult RemoveAssociation(string playerId, string teamId)
        {
            TeamEngineService.Instance.RemovePlayerOnTeam(playerId, teamId);

            RefreshWorkersOnTeamCache(teamId);

            return new EmptyResult();
        }

        /// <summary>
        /// Salva as associacoes de equipes e jogadores
        /// </summary>
        /// <param name="workersId"></param>
        /// <param name="teamId"></param>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        [Route("salvarAssociacoes")]
        [HttpPost]
        public ActionResult SaveAssociations(List<string> workersId, string teamId, int workerTypeId)
        {
            if(workersId != null)
            {
                List<PlayerEngineDTO> players;

                players = (from workerId in workersId
                           select new PlayerEngineDTO
                           {
                               Id = workerId
                           }).ToList();

                List<RunEngineDTO> runList = TeamEngineService.Instance.JoinPlayersOnTeam(teamId, players);

                RefreshWorkersOnTeamCache(teamId);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Carrega o datatable de jogadores de uma equipe
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Route("jogadores/{teamId}")]
        public ActionResult SearchPlayersFromTeam(JQueryDataTableRequest jqueryTableRequest, string teamId)
        {
            if (jqueryTableRequest != null)
            {
                List<string> allExternalWorkerIds = GetWorkersOnTeamCache();

                List<string> externalIds = (from externalId in allExternalWorkerIds select externalId).Skip(jqueryTableRequest.Page * 10).Take(10).ToList();

                List<WorkerDTO> workers = WorkerRepository.Instance.GetUserProfileByListOfExternalIds(externalIds);

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
                        RecordsTotal = allExternalWorkerIds.Count(),
                        RecordsFiltered = allExternalWorkerIds.Count(),
                        Data = workers.Select(r => new string[] { r.Name + ";" + r.LogoId.ToString(), r.Email, r.ExternalId }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = allExternalWorkerIds.Count(),
                        RecordsFiltered = allExternalWorkerIds.Count(),
                        Data = workers.Select(r => new string[] { r.Name + ";" + r.LogoId.ToString(), r.Email, r.ExternalId }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Relação de jogadores com uma equipe

        /// <summary>
        /// Carrega o datatable de jogadores para serem associados
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <param name="teamId"></param>
        /// <param name="workerTypeId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Route("carregarJogadores/{teamId}/{count:int}/{workerTypeId:int}")]
        public ActionResult LoadPlayersToAssociate(JQueryDataTableRequest jqueryTableRequest, string teamId, int count, int workerTypeId)
        {
            if (jqueryTableRequest != null)
            {
                TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);

                List<string> externalIds = GetWorkersOnTeamCache();
                externalIds.Add(team.MasterPlayerId);

                List<WorkerDTO> workers = WorkerRepository.Instance.GetUserProfileByWorkerTypeIdExceptWhenOnListOfExternalIds(externalIds, workerTypeId, jqueryTableRequest.Page);

                JQueryDataTableResponse response = new JQueryDataTableResponse()
                {
                    Draw = jqueryTableRequest.Draw,
                    RecordsTotal = count,
                    RecordsFiltered = count,
                    Data = workers.Select(r => new string[] { r.ExternalId, r.Name, r.Email }).ToArray()
                };

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Obtem a quantidade de funcionarios no datatable de jogadores para serem associados
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        [Route("obterQuantidadeParaAssociar/{workerTypeId:int}")]
        public ActionResult GetCountToAssociate(int workerTypeId)
        {
            List<string> externalIds = GetWorkersOnTeamCache();

            int count = WorkerRepository.Instance.GetCountByWorkerTypeId(externalIds, workerTypeId);

            return Json(JsonConvert.SerializeObject(count), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Cria a lista de seleção dos responsaveis
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private List<SelectListItem> GetSponsorsToSelect(string selected = null)
        {
            List<WorkerDTO> sponsors = new List<WorkerDTO>();

            sponsors = WorkerRepository.Instance.GetAllFromFirmByProfile(CurrentFirm.ExternalId, Profiles.LIDER);

            var query = from sponsor in sponsors
                        select new SelectListItem
                        {
                            Text = sponsor.Name,
                            Value = sponsor.ExternalId,
                            Selected = sponsor.ExternalId == selected
                        };

            if (query == null)
            {
                return new List<SelectListItem>();
            }

            return query.ToList();
        }

        /// <summary>
        /// Cria a lista de seleção dos responsaveis
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private List<SelectListItem> GetEpisodesToSelect(string selected = null)
        {
            GetAllDTO episodes;

            episodes = EpisodeEngineService.Instance.GetByGameIdAndActiveIsTrue(CurrentFirm.ExternalId);

            var query = from episode in episodes.List.episode
                        select new SelectListItem
                        {
                            Text = episode.Name,
                            Value = episode.Id,
                            Selected = episode.Id == selected
                        };

            if (query == null)
            {
                return new List<SelectListItem>();
            }

            return query.ToList();
        }

        private void RefreshWorkersOnTeamCache(string teamId)
        {
            HttpRuntime.Cache.Remove("WorkerExternalIdsOnTeam");

            TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);
            GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId);

            if (all != null)
            {
                List<string> externalWorkerIds = (from r in all.List.run where r.PlayerId != team.MasterPlayerId select r.PlayerId).ToList();
                HttpRuntime.Cache.Insert("WorkerExternalIdsOnTeam", externalWorkerIds);
            }
            else
            {
                HttpRuntime.Cache.Insert("WorkerExternalIdsOnTeam", new List<string>());
            }
        }

        private List<string> GetWorkersOnTeamCache()
        {
            return (List<string>)(HttpRuntime.Cache.Get("WorkerExternalIdsOnTeam"));
        }

        #endregion

    }
}