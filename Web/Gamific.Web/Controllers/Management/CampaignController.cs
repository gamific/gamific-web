using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/campanhas")]
    public class CampaignController : BaseController
    {
        // GET: Campaign
        [Route("")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE,JOGADOR")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("getCampaignById/{campaignId}")]
        [HttpGet]
        public ContentResult GetCampaign(string campaignId)
        {

            EpisodeEngineDTO rtn = EpisodeEngineService.Instance.GetById(campaignId);

            return Content(JsonConvert.SerializeObject(rtn), "application/json");
        }

        /**
        /// <summary>
        /// Abre o modal de associação de equipes com a campanha
        /// </summary>
        /// <returns></returns>
        [Route("associarEquipes")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
        public ActionResult AssociateTeams()
        {
            return PartialView("_AssociateTeams");
        }

        /// <summary>
        /// Abre a tela de edição de uma campanha
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [Route("editar/{campaignId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE,JOGADOR")]
        public ActionResult Edit(int campaignId)
        {
            CampaignEntity campaign = CampaignRepository.Instance.GetById(campaignId);

            ViewBag.CurrentProfile = CurrentProfile.ProfileName;

            ViewBag.CurrentWorker = CurrentWorker.Id;

            ViewBag.Profiles = GetProfilesToSelect(campaign.ProfileId);

            ViewBag.Sponsors = GetSponsorsToSelect(campaign.SponsorId);

            ViewBag.Metrics = MetricRepository.Instance.GetAllFromCampaign(campaignId);

            ViewBag.IconsDTO = GetIconsToSelect();
            ViewBag.IconsDTOString = GetIconsToSelect().Last().ConcatedString;

            return View("Edit", campaign);
        }

        /// <summary>
        /// Abre a tela de cadastro de uma campanha
        /// </summary>
        /// <returns></returns>
        [Route("cadastrar")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
        public ActionResult Create()
        {
            CampaignEntity campaign = new CampaignEntity();

            ViewBag.Profiles = GetProfilesToSelect(0);

            ViewBag.Sponsors = GetSponsorsToSelect(0);

            ViewBag.IconsDTO = GetIconsToSelect();
            ViewBag.IconsDTOString = GetIconsToSelect().Last().ConcatedString;

            return View("Edit", campaign);
        }

        /// <summary>
        /// Remove a associação entre campanha e equipe
        /// </summary>
        /// <param name="associationId"></param>
        /// <returns></returns>
        [Route("removerAssociacao/{associationId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
        public void RemoveAssociation(int associationId)
        {
            CampaignTeamEntity team = CampaignTeamRepository.Instance.GetById(associationId);

            team.Status = GenericStatus.INACTIVE;

            CampaignTeamRepository.Instance.UpdateCampaignTeam(team);

            CampaignEntity campaign = CampaignRepository.Instance.GetById(team.CampaignId);
        }

        /// <summary>
        /// Remove a metrica de uma campanha
        /// </summary>
        /// <param name="metricId"></param>
        /// <returns></returns>
        [Route("removerMetrica/{metricId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
        public string RemoveMetric(int metricId)
        {
            string msg = null;

            MetricEntity metric = MetricRepository.Instance.GetById(metricId);

            List<MetricEntity> metrics = MetricRepository.Instance.GetAllFromCampaign(metric.CampaignId);

            CampaignEntity campaign = null;

            campaign = CampaignRepository.Instance.GetById(metric.CampaignId);

            if (metrics.Count > 1)
            {
                metric.Status = GenericStatus.INACTIVE;

                MetricRepository.Instance.UpdateCampaignMetric(metric);
            }
            else
            {
                msg = "É necessário que exista pelo menos uma métrica cadastrada para campanha.";
            }

            return msg;
        }

        /// <summary>
        /// Remove a campanha
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [Route("remover/{campaignId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
        public ActionResult Remove(int campaignId)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    CampaignRepository.Instance.InactiveCampaign(campaignId);
                    Success("Registro removido com sucesso.");
                    scope.Complete();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    Error("Ocorreu um erro ao inativar a campanha");
                }
            }

            return View("Index");
        }

        /// <summary>
        /// Salva as informações da campanha sendo criada
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="teamsId"></param>
        /// <param name="metricaList"></param>
        /// <param name="goalList"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
        public ActionResult Save(CampaignEntity entity, string teamsId, List<MetricEntity> metricaList, string goalList)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    bool error = false;

                    entity.UpdatedBy = CurrentUserId;
                    entity.Status = GenericStatus.ACTIVE;
                    entity.FirmId = CurrentFirm.Id;

                    if (ModelState.IsValid)
                    {
                        if (entity.Id > 0)
                        {
                            ValidateModel(entity);

                            CampaignRepository.Instance.UpdateCampaign(entity);
                        }
                        else
                        {
                            ValidateModel(entity);

                            CampaignRepository.Instance.CreateCampaign(entity);
                        }

                        List<string> teamIdList = teamsId.Split(',').ToList();

                        foreach (string item in teamIdList)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {

                                CampaignTeamEntity campaignTeam = new CampaignTeamEntity();

                                campaignTeam.UpdatedBy = CurrentUserId;
                                campaignTeam.FirmId = CurrentFirm.Id;
                                campaignTeam.CampaignId = entity.Id;
                                campaignTeam.TeamId = int.Parse(item);
                                campaignTeam.Status = GenericStatus.ACTIVE;

                                campaignTeam = CampaignTeamRepository.Instance.CreateCampaignTeam(campaignTeam);

                                bool found = true;

                                List<string> goalListSplited = goalList.Split(',').ToList();

                                foreach (string goal in goalListSplited)
                                {
                                    if (!string.IsNullOrWhiteSpace(goal))
                                    {
                                        int teamId = int.Parse(goal.Split(';')[0]);
                                        int goalValue = string.IsNullOrWhiteSpace(goal.Split(';')[1]) ? 0 : int.Parse(goal.Split(';')[1]);
                                        if (goalValue > 0 && teamId > 0)
                                        {
                                            if (teamId == int.Parse(item))
                                            {
                                                TeamGoalEntity goalToSave = new TeamGoalEntity();

                                                goalToSave.FirmId = CurrentFirm.Id;
                                                goalToSave.Goal = goalValue;
                                                goalToSave.CampaignTeamId = campaignTeam.Id;
                                                goalToSave.UpdatedBy = CurrentUserId;

                                                TeamGoalRepository.Instance.CreateTeamGoal(goalToSave);
                                            }
                                        }
                                        else
                                        {
                                            if (goalValue <= 0 && teamId > 0 && !teamIdList.Contains(teamId.ToString()))
                                            {
                                                found = true;
                                            }
                                            else
                                            {
                                                found = false;
                                            }
                                        }
                                    }

                                }
                                if (!found)
                                {
                                    error = true;
                                    Error("Erro ao cadastrar a campanha. Todas as equipes devem possuir uma meta.");
                                    ViewBag.GoalList = goalList;
                                    ViewBag.TeamsId = teamsId;
                                }
                            }
                        }

                        for (int i = metricaList.Count - 1; i >= 0; i--)
                        {
                            if (metricaList[i].FlagDeleted == 1)
                            {
                                metricaList.Remove(metricaList[i]);
                            }
                        }

                        if (metricaList.Count < 1)
                        {
                            error = true;
                            Error("A campanha deve possuir pelo menos 1 métrica cadastrada.");
                            ViewBag.GoalList = goalList;
                            ViewBag.TeamsId = teamsId;
                        }
                        else
                        {
                            foreach (MetricEntity item in metricaList)
                            {
                                item.Icon = item.Icon.Replace("_", "-");
                                item.MetricName = item.MetricName.ToLowerInvariant();

                                if (ValidateMetric(item))
                                {
                                    if (item.Id > 0)
                                    {
                                        item.FirmId = CurrentFirm.Id;
                                        item.CampaignId = entity.Id;
                                        item.Status = GenericStatus.ACTIVE;
                                        item.UpdatedBy = CurrentUserId;

                                        MetricRepository.Instance.UpdateCampaignMetric(item);
                                    }
                                    else
                                    {
                                        item.FirmId = CurrentFirm.Id;
                                        item.CampaignId = entity.Id;
                                        item.Status = GenericStatus.ACTIVE;
                                        item.UpdatedBy = CurrentUserId;

                                        MetricRepository.Instance.CreateCampaignMetric(item);
                                    }
                                }
                                else
                                {
                                    error = true;
                                    Error("Alguns campos são obrigatórios para salvar as métricas.");
                                    ViewBag.GoalList = goalList;
                                    ViewBag.TeamsId = teamsId;
                                }

                            }

                        }

                        if (!error)
                        {
                            Success("Campanha atualizada com sucesso.");
                            scope.Complete();
                        }
                    }
                    else
                    {
                        Error("Alguns campos são obrigatórios para salvar a campanha.");
                        ViewBag.GoalList = goalList;
                        ViewBag.TeamsId = teamsId;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar salvar a campanha.", ex);
                ViewBag.GoalList = goalList;
                ViewBag.TeamsId = teamsId;
            }

            ViewBag.Profiles = GetProfilesToSelect(entity.ProfileId);

            ViewBag.Sponsors = GetSponsorsToSelect(entity.SponsorId);

            ViewBag.IconsDTO = GetIconsToSelect();
            ViewBag.IconsDTOString = GetIconsToSelect().Last().ConcatedString;

            ViewBag.Metrics = metricaList;

            ViewBag.CurrentProfile = CurrentProfile.ProfileName;

            ViewBag.CurrentWorker = CurrentWorker.Id;

            return View("Edit", entity);
        }

        /// <summary>
        /// Carrega o datatable de campanhas
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <returns></returns>
        [Route("search")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE,JOGADOR")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<CampaignEntity> searchResult = null;

                if (CurrentProfile.ProfileName.Equals("ADMINISTRADOR") || CurrentProfile.ProfileName.Equals("SUPERVISOR DE CAMPANHA"))
                {
                    searchResult = CampaignRepository.Instance.GetAllFromFirm(CurrentFirm.Id, jqueryTableRequest.Page, 10);
                }
                else if (CurrentProfile.ProfileName.Equals("SUPERVISOR DE EQUIPE"))
                {
                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId(CurrentUserId);

                    searchResult = new List<CampaignEntity>();

                    List<TeamEntity> teamsSponsor = TeamRepository.Instance.GetBySponsor(worker.Id);

                    foreach (TeamEntity item in teamsSponsor)
                    {
                        List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                        if (campaignsToAdd != null)
                        {
                            searchResult.AddRange(campaignsToAdd);
                        }
                    }

                    List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                    foreach (TeamEntity item in teamsPlaying)
                    {
                        List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                        if (campaignsToAdd != null)
                        {
                            searchResult.AddRange(campaignsToAdd);
                        }
                    }

                }
                else
                {
                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId(CurrentUserId);

                    searchResult = new List<CampaignEntity>();

                    List<TeamEntity> teamsPlaying = TeamRepository.Instance.GetAllFromWorker(worker.Id, CurrentFirm.Id);

                    foreach (TeamEntity item in teamsPlaying)
                    {
                        List<CampaignEntity> campaignsToAdd = CampaignRepository.Instance.GetAllFromTeam(item.Id);

                        if (campaignsToAdd != null)
                        {
                            searchResult.AddRange(campaignsToAdd);
                        }
                    }
                }

                var searchedQueryList = new List<CampaignEntity>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where n.CampaignName.ToLowerInvariant().Trim().Contains(filter)
                                        select n;

                    searchedQueryList = searchedQuery.ToList();
                }

                JQueryDataTableResponse response = new JQueryDataTableResponse()
                {
                    Draw = jqueryTableRequest.Draw,
                    RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                    RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                    Data = searchedQueryList.Select(r => new string[] { r.CampaignName, r.Id.ToString() }).ToArray()
                };

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega o datatable de equipes para serem associadas a campanha
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <param name="profileId"></param>
        /// <param name="campaignSponsorId"></param>
        /// <param name="campaignId"></param>
        /// <param name="teamsAlreadAssociated"></param>
        /// <returns></returns>
        [Route("associarEquipes/{profileId:int}/{campaignSponsorId:int}/{campaignId:int}/{teamsAlreadAssociated}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
        public ActionResult LoadTeamsToAssociate(JQueryDataTableRequest jqueryTableRequest, int profileId, int campaignSponsorId, int campaignId, string teamsAlreadAssociated)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<TeamDTO> searchResult = null;

                searchResult = TeamRepository.Instance.GetAllFromFirmWithoutCampaignByProfileWithoutCampaignSponsor(CurrentFirm.Id, profileId, campaignSponsorId, campaignId, jqueryTableRequest.Page, 10);

                var searchedQueryList = new List<TeamDTO>();

                searchedQueryList = searchResult;

                List<string> teamsIdToRemoveList = teamsAlreadAssociated.Split(',').ToList();
                List<int> teamsIdToRemoveIntList = new List<int>();

                foreach (string idString in teamsIdToRemoveList)
                {
                    if (!string.IsNullOrWhiteSpace(idString))
                    {
                        teamsIdToRemoveIntList.Add(int.Parse(idString));
                    }
                }

                if (teamsIdToRemoveIntList.Count > 0)
                {
                    for (int i = searchedQueryList.Count - 1; i >= 0; i--)
                    {
                        if (teamsIdToRemoveIntList.Contains(searchedQueryList[i].IdTeam))
                        {
                            searchedQueryList.Remove(searchedQueryList[i]);
                        }
                    }
                }


                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where n.TeamName.ToLowerInvariant().Trim().Contains(filter)
                                        select n;

                    searchedQueryList = searchedQuery.ToList();
                }

                JQueryDataTableResponse response = new JQueryDataTableResponse()
                {
                    Draw = jqueryTableRequest.Draw,
                    RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                    RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                    Data = searchedQueryList.Select(r => new string[] { r.IdTeam.ToString(), r.TeamName, r.SponsorName, r.IdTeam.ToString() }).ToArray()
                };

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega o datatable de equipes da campanha sendo adicionadas
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <param name="campaingId"></param>
        /// <param name="teamsIdList"></param>
        /// <returns></returns>
        [Route("carregarEquipes/{campaingId:int}/{teamsIdList}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE,JOGADOR")]
        public ActionResult LoadTeamsBeingAdded(JQueryDataTableRequest jqueryTableRequest, int campaingId, string teamsIdList)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<TeamDTO> searchResult = null;

                searchResult = TeamRepository.Instance.GetAllFromCampaign(CurrentFirm.Id, campaingId, jqueryTableRequest.Page, 10);

                if (!string.IsNullOrWhiteSpace(teamsIdList) && !teamsIdList.Equals("!key!"))
                {
                    String teamsIdFormatted = teamsIdList.Substring(0, teamsIdList.Length - 1);
                    if (!string.IsNullOrWhiteSpace(teamsIdFormatted))
                    {
                        searchResult.AddRange(TeamRepository.Instance.GetDTOsByIds(CurrentFirm.Id, teamsIdFormatted));
                    }
                }

                var searchedQueryList = new List<TeamDTO>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where n.TeamName.ToLowerInvariant().Trim().Contains(filter)
                                        select n;

                    searchedQueryList = searchedQuery.ToList();
                }

                JQueryDataTableResponse response = new JQueryDataTableResponse()
                {
                    Draw = jqueryTableRequest.Draw,
                    RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                    RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                    Data = searchedQueryList.Select(r => new string[] { r.TeamName + ";" + r.LogoId, r.SponsorName, r.IdTeam.ToString() + ";" + r.IdAssociation.ToString() + ";" + r.SponsorId.ToString(), r.IdAssociation.ToString() + ";" + r.IdTeam.ToString() }).ToArray()
                };

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cria a lista de seleção dos perfis
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private List<SelectListItem> GetProfilesToSelect(int selected)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            List<SelectListItem> listRtn = new List<SelectListItem>();

            if (selected > 0)
            {
                WorkerTypeEntity profile = WorkerTypeRepository.Instance.GetById(selected);

                listRtn.Add(new SelectListItem
                {
                    Text = profile.ProfileName,
                    Value = profile.Id.ToString(),
                    Selected = profile.Id == selected
                });
            }
            else
            {
                List<WorkerTypeEntity> profiles = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.Id);

                var query = from c in profiles
                            select new SelectListItem
                            {
                                Text = c.ProfileName,
                                Value = c.Id.ToString(),
                                Selected = c.Id == selected
                            };

                listRtn = query.ToList();
            }

            return listRtn;
        }

        /// <summary>
        /// Cria a lista de seleção dos icones
        /// </summary>
        /// <returns></returns>
        private List<IconsDTO> GetIconsToSelect()
        {
            List<IconsDTO> icons = new List<IconsDTO>();

            IconsDTO dtoSelect = new IconsDTO();

            dtoSelect.Value = "Selecione";

            icons.Add(dtoSelect);

            string concatedString = "Selecione;";

            for (int i = 0; i < Enum.GetValues(typeof(Icons)).Length; i++)
            {
                IconsDTO dto = new IconsDTO();
                concatedString += Enum.GetValues(typeof(Icons)).GetValue(i).ToString() + ";";
                dto.ConcatedString = concatedString;
                dto.Value = Enum.GetValues(typeof(Icons)).GetValue(i).ToString();

                icons.Add(dto);
            }

            return icons;
        }

        /// <summary>
        /// Valida a metrica
        /// </summary>
        /// <param name="metric"></param>
        /// <returns></returns>
        private bool ValidateMetric(MetricEntity metric)
        {
            return !string.IsNullOrWhiteSpace(metric.Icon) && !metric.Icon.Equals("Selecione") && !string.IsNullOrWhiteSpace(metric.MetricName) && metric.Weigth > 0;
        }

        /// <summary>
        /// Cria a lista de seleção dos responsaveis
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private List<SelectListItem> GetSponsorsToSelect(int selected)
        {
            if (selected < 0)
            {
                selected = 0;
            }

            List<WorkerDTO> sponsors = new List<WorkerDTO>();

            WorkerTypeEntity profile = WorkerTypeRepository.Instance.GetByName("SUPERVISOR DE CAMPANHA", CurrentFirm.Id);

            WorkerTypeEntity profile1 = WorkerTypeRepository.Instance.GetByName("ADMINISTRADOR", CurrentFirm.Id);

            sponsors = WorkerRepository.Instance.GetAllByProfileIdAndFirmId(profile.Id, CurrentFirm.Id);

            List<WorkerDTO> sponsorsToAdd = WorkerRepository.Instance.GetAllByProfileIdAndFirmId(profile1.Id, CurrentFirm.Id);

            if (sponsorsToAdd != null)
            {
                sponsors.AddRange(sponsorsToAdd);
            }

            var query = from c in sponsors
                        select new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.IdWorker.ToString(),
                            Selected = c.IdWorker == selected
                        };

            return query.ToList();
        }
    **/
    }
}