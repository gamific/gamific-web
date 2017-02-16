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
                GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId, jqueryTableRequest.Page);

                List<string> externalIds = all.List.run.Select(x => x.PlayerId).ToList();

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
                        RecordsTotal = all.List.run.Count(),
                        RecordsFiltered = all.List.run.Count(),
                        Data = workers.Select(r => new string[] { r.Name + ";" + r.LogoId, r.Email, r.WorkerTypeName, r.ExternalId }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.List.run.Count(),
                        RecordsFiltered = all.List.run.Count(),
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
            PlayerEngineDTO player = PlayerEngineService.Instance.GetById(playerId);
            ViewBag.WorkerName = player.Nick;

            List<WorkerTypeMetricDTO> metricsWorkerType = WorkerTypeMetricRepository.Instance.GetAllFromWorkerByPlayerId(playerId);

            List<MetricEntity> metrics = new List<MetricEntity>();

            foreach (WorkerTypeMetricDTO metric in metricsWorkerType)
            {
                MetricEngineDTO m = MetricEngineService.Instance.GetById(metric.MetricExternalId);

                metrics.Add(new MetricEntity{
                    MetricName = m.Name,
                    Icon = m.Icon,
                    ExternalID = m.Id,
                });
            }

            RunEngineDTO run = RunEngineService.Instance.GetRunByPlayerAndTeamId(playerId, teamId);

            List<GoalDTO> goals = GoalRepository.Instance.GetAllFromWorkerByRunId(run.Id, metrics);

            foreach(GoalDTO goal in goals)
            {
                if(goal.EpisodeId == null)
                {
                    goal.EpisodeId = episodeId;
                }
            }

            return PartialView("_Edit", goals);
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(List<GoalEntity> goalList)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (GoalEntity goal in goalList)
                    {
                        goal.UpdatedBy = CurrentUserId;
                        if(goal.Id == 0 && goal.Goal > 0)
                        {
                            GoalRepository.Instance.CreateGoal(goal);
                        }
                        else if(goal.Goal >= 0 && goal.Id > 0)
                        {
                            GoalRepository.Instance.UpdateGoal(goal);
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
           [Route("cadastrar/")]
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

        /// <summary>
        /// Salva as informações do resultado via arquivo
        /// </summary>
        /// <param name="goalArchive"></param>
        /// <returns></returns>
        [Route("salvarArquivoMeta")]
        [HttpPost]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE")]
        public ActionResult SaveGoalArchive(HttpPostedFileBase goalArchive, string episodeId)
        {
            try
            {
                goalArchive.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), goalArchive.FileName));

                string path = Path.Combine(Server.MapPath("~/App_Data"), goalArchive.FileName);

                var archive = new ExcelQueryFactory(path);

                var rows = from x in archive.WorksheetRange("A1", "D" + rowsCount, "Goals")
                           select x;

                foreach (var row in rows)
                {
                    if (row[0] == null || row[0].ToString().Equals("") || row[3].ToString().Equals("0"))
                    {
                        continue;
                    }

                    MetricEngineDTO metric = MetricEngineService.Instance.GetDTOByGameAndName(CurrentFirm.ExternalId, row[1].ToString());

                    if (metric == null)
                    {
                        continue;
                    }

                    UserProfileEntity user = UserProfileRepository.Instance.GetByEmail(row[0].Value.ToString());

                    if (user == null)
                    {
                        continue;
                    }

                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId(int.Parse(user.Id.ToString()));

                    if (worker == null)
                    {
                        continue;
                    }

                    TeamEngineDTO team = TeamEngineService.Instance.GetByEpisodeIdAndNick(episodeId, row[2].Value.ToString());

                    if (team == null)
                    {
                        continue;
                    }

                    RunEngineDTO run = RunEngineService.Instance.GetRunByPlayerAndTeamId(worker.ExternalId, team.Id);

                    if (run == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(row[0].ToString()) && !string.IsNullOrWhiteSpace(row[1].ToString()) && !string.IsNullOrWhiteSpace(row[2].ToString()) && !string.IsNullOrWhiteSpace(row[3].ToString()))
                    {
                        GoalDTO goalDTO;

                        goalDTO = GoalRepository.Instance.GetByRunIdAndMetricId(run.Id, metric.Id);

                        if (goalDTO != null)
                        {
                            GoalEntity goal = new GoalEntity
                            {
                                RunId = run.Id,
                                ExternalMetricId = metric.Id,
                                EpisodeId = episodeId,
                                Id = goalDTO.GoalId,
                                Goal = Int32.Parse(row[3].Value.ToString())
                            };

                            GoalRepository.Instance.UpdateGoal(goal);
                        }
                        else
                        {
                            GoalEntity goal = new GoalEntity
                            {
                                RunId = run.Id,
                                ExternalMetricId = metric.Id,
                                EpisodeId = episodeId,
                                Goal = Int32.Parse(row[3].Value.ToString())
                            };

                            GoalRepository.Instance.CreateGoal(goal);
                        }
                        
                    }

                    
                }

                Success("Metas cadastradas com sucesso.");

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar as metas.");

                return PartialView("_EditAll");
            }

            return new EmptyResult();
        }
    }
}