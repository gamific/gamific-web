using Aspose.Cells;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vlast.Gamific.Model.Account.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;
using System.Text;
using Vlast.Gamific.Model.Account.Domain;
using LinqToExcel;
using System.Web;
using Vlast.Broker.EMAIL;
using Vlast.Util.Parameter;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/metas")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class GoalController : BaseController
    {

        public static int rowsCount;

        // GET: Goal
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("search/{teamId}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, string teamId)
        {
            if (jqueryTableRequest != null)
            {
                TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);
                GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId, jqueryTableRequest.Page);

                List<string> externalIds = all.List.run.Select(x => x.PlayerId).Where(q => q != team.MasterPlayerId).ToList();

                List<WorkerDTO> workers = WorkerRepository.Instance.GetDTOFromListExternalId(externalIds);

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
                        RecordsTotal = workers.Count(),
                        RecordsFiltered = workers.Count(),
                        Data = workers.Select(r => new string[] { r.Name + ";" + r.LogoId, r.Email, r.WorkerTypeName, r.ExternalId }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = workers.Count(),
                        RecordsFiltered = workers.Count(),
                        Data = workers.Select(r => new string[] { r.Name + ";" + r.LogoId, r.Email, r.WorkerTypeName, r.ExternalId }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
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

        [Route("editar/{playerId}/{teamId}/{episodeId}")]
        public ActionResult Edit(string playerId, string teamId, string episodeId)
        {
            try
            {
                PlayerEngineDTO player = PlayerEngineService.Instance.GetById(playerId);
                ViewBag.WorkerName = player.Nick;

                List<WorkerTypeMetricDTO> metricsWorkerType = WorkerTypeMetricRepository.Instance.GetAllFromWorkerByPlayerId(playerId);

                List<MetricEngineDTO> metrics = new List<MetricEngineDTO>();

                foreach (WorkerTypeMetricDTO metric in metricsWorkerType)
                {
                    try
                    {
                        MetricEngineDTO m = MetricEngineService.Instance.GetById(metric.MetricExternalId);
                        metrics.Add(m);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }

                RunEngineDTO run = RunEngineService.Instance.GetRunByPlayerAndTeamId(playerId, teamId);

                List<GoalEngineDTO> goals = new List<GoalEngineDTO>();
                foreach (MetricEngineDTO metric in metrics)
                {
                    try
                    {
                        GoalEngineDTO goal = GoalEngineService.Instance.GetByRunIdAndMetricId(run.Id, metric.Id);
                        goals.Add(goal);
                    }
                    catch(Exception ex)
                    {
                        GoalEngineDTO goal = new GoalEngineDTO
                        {
                            Goal = 0,
                            MetricId = metric.Id,
                            RunId = run.Id,
                            MetricIcon = metric.Icon,
                            MetricName = metric.Name,
                        };
                        goals.Add(goal);
                        Logger.LogException(ex);
                    }

                }
                

                return PartialView("_Edit", goals);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar adicionar uma meta.", ex);
            }

            return Redirect("/admin/metas");
        }



        [Route("editarEquipe/{teamId}/{episodeId}")]
        public ActionResult EditTeam(string teamId, string episodeId)
        {
            try
            {
                TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);
                ViewBag.TeamName = team.Nick;

                List<MetricEngineDTO> metricsDTO = MetricEngineService.Instance.GetByGameId(team.GameId).List.metric;

                List<GoalEngineDTO> goals = GoalEngineService.Instance.GetByTeamId(teamId).List.goal;

                if (goals.Count < 1)
                {
                    foreach (MetricEngineDTO m in metricsDTO)
                    {
                        GoalEngineDTO dto = new GoalEngineDTO();

                        dto.MetricIcon = m.Icon;
                        dto.MetricId = m.Id;
                        dto.MetricName = m.Name;
                        dto.TeamId = teamId;

                        goals.Add(dto);
                    }
                }
                else
                {
                    List<string> metricIds = new List<string>();

                    foreach (GoalEngineDTO g in goals)
                    {
                        metricIds.Add(g.MetricId);
                    }

                    foreach (MetricEngineDTO m in metricsDTO)
                    {
                        if (!metricIds.Contains(m.Id))
                        {
                            GoalEngineDTO dto = new GoalEngineDTO();

                            dto.MetricIcon = m.Icon;
                            dto.MetricId = m.Id;
                            dto.MetricName = m.Name;
                            dto.TeamId = teamId;

                            goals.Add(dto);
                        }
                    }
                }

                return PartialView("_EditTeam", goals);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar adicionar uma meta.", ex);
            }

            return Redirect("/admin/metas");
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(List<GoalEngineDTO> goalList)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (GoalEngineDTO goal in goalList)
                    {
                        if (goal.Id == null && goal.Goal > 0)
                        {
                            MetricEngineDTO metric = MetricEngineService.Instance.GetById(goal.MetricId);
                            GoalEngineDTO goalEngine = new GoalEngineDTO
                            {
                                Goal = goal.Goal,
                                MetricIcon = metric.Icon,
                                MetricId = metric.Id,
                                MetricName = metric.Name,
                                RunId = goal.RunId
                            };
                            GoalEngineService.Instance.CreateOrUpdate(goalEngine);
                        }
                        else if (goal.Goal > 0)
                        {
                            GoalEngineDTO goalEngine = GoalEngineService.Instance.GetByRunIdAndMetricId(goal.RunId, goal.MetricId);
                            goalEngine.Goal = goal.Goal;
                            GoalEngineService.Instance.CreateOrUpdate(goalEngine);
                        }

                    }

                    Success("Associação feita com sucesso.");
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar adicionar uma meta.", ex);
            }

            return Redirect("/admin/metas");
        }

        [Route("salvarEquipe")]
        [HttpPost]
        public ActionResult SaveTeam(List<GoalEngineDTO> goalList)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (GoalEngineDTO goal in goalList)
                    {
                        if (string.IsNullOrWhiteSpace(goal.Id) && goal.Goal > 0)
                        {
                            GoalEngineService.Instance.CreateOrUpdate(goal);
                        }
                        else if (!string.IsNullOrWhiteSpace(goal.Id))
                        {
                            GoalEngineService.Instance.CreateOrUpdate(goal);
                        }
                    }

                    Success("Associação feita com sucesso.");
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar adicionar uma meta.", ex);
            }

            return Redirect("/admin/metas");
        }

        /// <summary>
        /// Abre o modal para cadastro de uma meta padrao
        /// </summary>
        /// <returns></returns>
        [Route("cadastrar")]
        public ActionResult Create(/*List<GoalEntity> goalList, string gameId*/)
        {
            GoalDTO goals = new GoalDTO();



            return PartialView("_EditAll", goals);
        }


        /// <summary>
        /// Abre o popup de cadastrar resultados via arquivo
        /// </summary>
        /// <returns></returns>
        [Route("cadastrarArquivoMeta/{episodeId}")]
        public ActionResult CreateGoalArchive(string episodeId)
        {
            ViewBag.Episodes = GetEpisodesToSelect(episodeId);
            ViewBag.EpisodeId = episodeId;

            return PartialView("_EditAll");
        }


        /// <summary>
        /// Retorna a planiha de resultados para ser preenchida
        /// </summary>
        /// <returns></returns>
        [Route("baixarPlanilhaMeta/{episodeId}")]
        [HttpGet]
        public FileContentResult DownloadPlanGoal(string episodeId)
        {

            List<MetricEngineDTO> metrics = MetricEngineService.Instance.GetByGameId(CurrentFirm.ExternalId).List.metric;

            var workbook = new Workbook();

            

            var worksheetResults = workbook.Worksheets[0];

            rowsCount = 5000;

            worksheetResults.Cells.HideColumns(4, 16384);
            worksheetResults.Cells.HideRows(rowsCount, 1048576);
            worksheetResults.Cells.StandardWidth = 35.0;

            worksheetResults.Name = "Goals";
            workbook.FileName = "Planilha-de-Metas.xls";

            var cellsResults = worksheetResults.Cells;

            cellsResults["A1"].PutValue("Email");
            cellsResults["B1"].PutValue("Métrica");
            cellsResults["C1"].PutValue("Equipe");
            cellsResults["D1"].PutValue("Meta");


            List<string> metricsNames = new List<string>();

            foreach (MetricEngineDTO metric in metrics)
            {
                metricsNames.Add(metric.Name);
            }

            List<string> teamNames = new List<string>();

            GetAllDTO all = TeamEngineService.Instance.FindByEpisodeId(episodeId);

            foreach (TeamEngineDTO team in all.List.team)
            {
                teamNames.Add(team.Nick);
            }

            var flatListMetrics = string.Join(",", metricsNames.ToArray());

            var flatListTeams = string.Join(",", teamNames.ToArray());

            var validations = worksheetResults.Validations;

            var validationEmail = validations[validations.Add()];
            validationEmail.Type = ValidationType.TextLength;
            validationEmail.Operator = OperatorType.None;
            validationEmail.InCellDropDown = false;
            validationEmail.ShowError = true;
            validationEmail.AlertStyle = ValidationAlertType.Stop;
            CellArea areaEmails;
            areaEmails.StartRow = 1;
            areaEmails.EndRow = rowsCount;
            areaEmails.StartColumn = 0;
            areaEmails.EndColumn = 0;
            validationEmail.AreaList.Add(areaEmails);

            var validationMetric = validations[validations.Add()];
            validationMetric.Type = ValidationType.List;
            validationMetric.Operator = OperatorType.Between;
            validationMetric.InCellDropDown = true;
            validationMetric.ShowError = true;
            validationMetric.Formula1 = flatListMetrics;
            validationMetric.AlertStyle = ValidationAlertType.Stop;
            CellArea areaMetrics;
            areaMetrics.StartRow = 1;
            areaMetrics.EndRow = rowsCount;
            areaMetrics.StartColumn = 1;
            areaMetrics.EndColumn = 1;
            validationMetric.AreaList.Add(areaMetrics);

            var validationTeam = validations[validations.Add()];
            validationTeam.Type = ValidationType.List;
            validationTeam.Operator = OperatorType.Between;
            validationTeam.InCellDropDown = true;
            validationTeam.ShowError = true;
            validationTeam.Formula1 = flatListTeams;
            validationTeam.AlertStyle = ValidationAlertType.Stop;
            CellArea areaTeam;
            areaTeam.StartRow = 1;
            areaTeam.EndRow = rowsCount;
            areaTeam.StartColumn = 2;
            areaTeam.EndColumn = 2;
            validationTeam.AreaList.Add(areaTeam);

            var validationResult = validations[validations.Add()];
            validationResult.Type = ValidationType.WholeNumber;
            validationResult.Operator = OperatorType.Between;
            validationResult.Formula1 = 0.ToString();
            validationResult.Formula2 = Int32.MaxValue.ToString();
            validationResult.InCellDropDown = false;
            validationResult.ShowError = true;
            validationResult.AlertStyle = ValidationAlertType.Stop;
            CellArea areaResult;
            areaResult.StartRow = 1;
            areaResult.EndRow = rowsCount;
            areaResult.StartColumn = 3;
            areaResult.EndColumn = 3;
            validationResult.AreaList.Add(areaResult);



            MemoryStream ms = new MemoryStream();

            

            ms = workbook.SaveToStream();


            return File(ms.ToArray(), "application/vnd.ms-excel");
        }

        [Route("salvarArquivoMeta")]
        [HttpPost]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE")]
        public ActionResult SaveGoalArchive(HttpPostedFileBase goalArchive, string episodeId)
        {
            if (CurrentFirm.ExternalId == "588602233a87786bec6ca703") //Syngenta
            {
                return SaveGoalArchiveSyngenta(goalArchive, episodeId);
            }
            else
            {
                return SaveGoalArchive(goalArchive, episodeId);
            }


            return Json(new { Success = false }, JsonRequestBehavior.DenyGet);
        }


        /// <summary>
        /// Salva as informações do resultado via arquivo
        /// </summary>
        /// <param name="goalArchive"></param>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        private ActionResult SaveGoalArchiveSyngenta(HttpPostedFileBase goalArchive, string episodeId)
        {
            string errors = "Quantidade de erros: {0}<br/>Última linha lida: {1}<br/>";
            int line = 1;
            int countErrors = 0;
            int countEmptyLines = 0;

            int CODIGO_TERRITORIO = 0;
            int RESPONSAVEL = 1;
            int EMAIL = 2;
            int REG = 3;
            int PROMOxISNT = 4;
            int DIA = 5;
            int CULTURA = 6;
            int NOME_PADRAO = 7;
            int TOTAL = 8;

            try
            {
                goalArchive.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), goalArchive.FileName));

                string path = Path.Combine(Server.MapPath("~/App_Data"), goalArchive.FileName);

                var archive = new ExcelQueryFactory(path);

                var rows = from x in archive.WorksheetRange("A1", "I" + rowsCount, "META")
                           select x;

                foreach (var row in rows)
                {
                    line++;

                    if (countEmptyLines >= 3)
                    {
                        break;
                    }

                    if (row[0] == null || row[0].ToString().Equals("") || row[1] == null || row[1].ToString().Equals(""))
                    {
                        countEmptyLines++;
                        continue;
                    }

                    countEmptyLines = 0;

                    if (row[TOTAL].ToString().Equals("0"))
                    {
                        continue;
                    }

                    MetricEngineDTO metric = new MetricEngineDTO();
                    TeamEngineDTO team = new TeamEngineDTO();
                    RunEngineDTO run = new RunEngineDTO();



                    try
                    {
                        metric = MetricEngineService.Instance.GetDTOByGameAndName(CurrentFirm.ExternalId, row[NOME_PADRAO].ToString());
                    }
                    catch (Exception e)
                    {
                        errors += "Erro na coluna 2 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    UserProfileEntity user = UserProfileRepository.Instance.GetByEmail(row[EMAIL].Value.ToString());

                    if (user == null)
                    {
                        errors += "Erro na coluna 1 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId(int.Parse(user.Id.ToString()));

                    if (worker == null)
                    {
                        errors += "Erro na coluna 1 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    try
                    {
                        team = TeamEngineService.Instance.GetByEpisodeIdAndNick(episodeId, row[REG].Value.ToString());
                    }
                    catch (Exception e)
                    {
                        errors += "Erro na coluna 3 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    try
                    {
                        run = RunEngineService.Instance.GetRunByPlayerAndTeamId(worker.ExternalId, team.Id);
                    }
                    catch (Exception e)
                    {
                        errors += "Jogador " + user.Name + " não está cadastrado no time " + team.Nick + ". Linha: " + line + "<br/>";
                        countErrors++;
                        continue;
                    }


                    if (!string.IsNullOrWhiteSpace(row[0].ToString()) && !string.IsNullOrWhiteSpace(row[1].ToString()) && !string.IsNullOrWhiteSpace(row[2].ToString()) && !string.IsNullOrWhiteSpace(row[3].ToString()))
                    {
                        GoalEngineDTO goalEngineDTO;


                        try
                        {
                            goalEngineDTO = GoalEngineService.Instance.GetByRunIdAndMetricId(run.Id, metric.Id);
                            goalEngineDTO.Goal = Int32.Parse(row[3].Value.ToString());
                        }
                        catch (Exception e)
                        {
                            goalEngineDTO = new GoalEngineDTO
                            {
                                RunId = run.Id,
                                MetricId = metric.Id,
                                MetricIcon = metric.Icon,
                                MetricName = metric.Name,
                                Goal = Int32.Parse(row[TOTAL].Value.ToString()),
                                ItemId = "",
                                Percentage = 0
                            };
                        }

                        goalEngineDTO = GoalEngineService.Instance.CreateOrUpdate(goalEngineDTO);
                    }
                }

                errors = string.Format(errors, countErrors, line);

                string emailFrom = ParameterCache.Get("SUPPORT_EMAIL");
                string subject = countErrors >= 1 ? "Erros ao subir planilha de metas" : "O lançamento de metas foi um sucesso.";
                subject = CurrentFirm.FirmName + ": " + subject;
                bool r = EmailDispatcher.SendEmail(emailFrom, subject, new List<string>() { emailFrom, CurrentUserProfile.Email }, errors);

                return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { Success = false }, JsonRequestBehavior.DenyGet);
            }
        }



        /// <summary>
        /// Salva as informações do resultado via arquivo
        /// </summary>
        /// <param name="goalArchive"></param>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        private ActionResult SaveGoalArchiveDefault(HttpPostedFileBase goalArchive, string episodeId)
        {
            string errors = "Quantidade de erros: {0}<br/>Última linha lida: {1}<br/>";
            int line = 1;
            int countErrors = 0;
            int countEmptyLines = 0;

            try
            {
                goalArchive.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), goalArchive.FileName));

                string path = Path.Combine(Server.MapPath("~/App_Data"), goalArchive.FileName);

                var archive = new ExcelQueryFactory(path);

                var rows = from x in archive.WorksheetRange("A1", "D" + rowsCount, "Goals")
                           select x;

                foreach (var row in rows)
                {
                    line++;

                    if (countEmptyLines >= 3)
                    {
                        break;
                    }

                    if (row[0] == null || row[0].ToString().Equals("") || row[1] == null || row[1].ToString().Equals(""))
                    {
                        countEmptyLines++;
                        continue;
                    }

                    countEmptyLines = 0;

                    if (row[3].ToString().Equals("0"))
                    {
                        continue;
                    }

                    MetricEngineDTO metric = new MetricEngineDTO();
                    TeamEngineDTO team = new TeamEngineDTO();
                    RunEngineDTO run = new RunEngineDTO();



                    try
                    {
                        metric = MetricEngineService.Instance.GetDTOByGameAndName(CurrentFirm.ExternalId, row[1].ToString());
                    }
                    catch (Exception e)
                    {
                        errors += "Erro na coluna 2 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    UserProfileEntity user = UserProfileRepository.Instance.GetByEmail(row[0].Value.ToString());

                    if (user == null)
                    {
                        errors += "Erro na coluna 1 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId(int.Parse(user.Id.ToString()));

                    if (worker == null)
                    {
                        errors += "Erro na coluna 1 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    try
                    {
                        team = TeamEngineService.Instance.GetByEpisodeIdAndNick(episodeId, row[2].Value.ToString());
                    }
                    catch (Exception e)
                    {
                        errors += "Erro na coluna 3 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    try
                    {
                        run = RunEngineService.Instance.GetRunByPlayerAndTeamId(worker.ExternalId, team.Id);
                    }
                    catch (Exception e)
                    {
                        errors += "Jogador " + user.Name + " não está cadastrado no time " + team.Nick + ". Linha: " + line + "<br/>";
                        countErrors++;
                        continue;
                    }


                    if (!string.IsNullOrWhiteSpace(row[0].ToString()) && !string.IsNullOrWhiteSpace(row[1].ToString()) && !string.IsNullOrWhiteSpace(row[2].ToString()) && !string.IsNullOrWhiteSpace(row[3].ToString()))
                    {
                        GoalEngineDTO goalEngineDTO;


                        try
                        {
                            goalEngineDTO = GoalEngineService.Instance.GetByRunIdAndMetricId(run.Id, metric.Id);
                            goalEngineDTO.Goal = Int32.Parse(row[3].Value.ToString());
                        }
                        catch (Exception e)
                        {
                            goalEngineDTO = new GoalEngineDTO
                            {
                                RunId = run.Id,
                                MetricId = metric.Id,
                                MetricIcon = metric.Icon,
                                MetricName = metric.Name,
                                Goal = Int32.Parse(row[3].Value.ToString()),
                                ItemId = "",
                                Percentage = 0
                            };
                        }

                        goalEngineDTO.Goal = Int32.Parse(row[3].Value.ToString());
                        goalEngineDTO = GoalEngineService.Instance.CreateOrUpdate(goalEngineDTO);
                    }
                }

                errors = string.Format(errors, countErrors, line);

                string emailFrom = ParameterCache.Get("SUPPORT_EMAIL");
                string subject = countErrors >= 1 ? "Erros ao subir planilha de metas" : "O lançamento de metas foi um sucesso.";
                subject = CurrentFirm.FirmName + ": " + subject;
                bool r = EmailDispatcher.SendEmail(emailFrom, subject, new List<string>() { emailFrom, CurrentUserProfile.Email }, errors);

                return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { Success = false }, JsonRequestBehavior.DenyGet);
            }
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
    }
}