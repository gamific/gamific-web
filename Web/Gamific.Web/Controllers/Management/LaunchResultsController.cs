using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vlast.Gamific.Model.Account.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;
using Vlast.Gamific.Model.Public.DTO;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Newtonsoft.Json;
using System.Web;
using Aspose.Cells;
using System.IO;
using LinqToExcel;
using Vlast.Gamific.Model.Account.Domain;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/lancarResultados")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class LaunchResultsController : BaseController
    {

        public static int rowsCount;

        [Route("")]
        public ActionResult Index()
        {
            ViewBag.NumberOfPlayers = WorkerRepository.Instance.GetCountByProfileFromFirm(CurrentFirm.Id, Profiles.JOGADOR);

            return View();
        }

        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
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
                        Data = workers.Select(r => new string[] { r.Name + ";" + r.LogoId, r.Email, r.WorkerTypeName, r.ExternalId }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = allExternalWorkerIds.Count(),
                        RecordsFiltered = allExternalWorkerIds.Count(),
                        Data = workers.Select(r => new string[] { r.Name + ";" + r.LogoId, r.Email, r.WorkerTypeName, r.ExternalId }).ToArray().OrderByDescending(item => item[index]).ToArray()

                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("edit/{teamId}/{playerId}")]
        public ActionResult Edit(string teamId, string playerId)
        {
            ViewBag.Player = PlayerEngineService.Instance.GetById(playerId);
            ViewBag.Now = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Run = RunEngineService.Instance.GetRunByPlayerAndTeamId(playerId, teamId);

            List<WorkerTypeMetricDTO> metricsWorkerType = WorkerTypeMetricRepository.Instance.GetAllFromWorkerByPlayerId(playerId);

            List<MetricEngineDTO> metrics = new List<MetricEngineDTO>();

            foreach (WorkerTypeMetricDTO metric in metricsWorkerType)
            {
                metrics.Add(MetricEngineService.Instance.GetById(metric.MetricExternalId));
            }

            return PartialView("_Edit", metrics);
        }


        [Route("atualizarEquipe/{teamId}")]
        public ActionResult UpdateTeam(string teamId)
        {
            RefreshWorkersOnTeamCache(teamId);

            return new EmptyResult();
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


        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(List<RunMetricEngineDTO> resultList, string runId, string date)
        {
            DateTime dateTime = Convert.ToDateTime(date);
            long time = dateTime.Ticks;

            foreach (RunMetricEngineDTO result in resultList)
            {
                if (result.Points > 0)
                {
                    result.Score = 0;
                    result.RunId = runId;
                    result.Date = time;
                    RunMetricEngineService.Instance.CreateOrUpdate(result);
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        private void RefreshWorkersOnTeamCache(string teamId)
        {
            HttpRuntime.Cache.Remove("WorkerExternalIdsOnTeam");

            GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId);

            if (all != null)
            {
                List<string> externalWorkerIds = (from r in all.List.run select r.PlayerId).ToList();
                HttpRuntime.Cache.Insert("WorkerExternalIdOnTeam", externalWorkerIds);
            }
            else
            {
                HttpRuntime.Cache.Insert("WorkerExternalIdOnTeam", new List<string>());
            }
        }

        private List<string> GetWorkersOnTeamCache()
        {
            return (List<string>)(HttpRuntime.Cache.Get("WorkerExternalIdOnTeam"));
        }

        /// <summary>
        /// Abre o popup de cadastrar resultados via arquivo
        /// </summary>
        /// <returns></returns>
        [Route("cadastrarResultadoArquivo/{episodeId}")]
        public ActionResult CreateResultArchive(string episodeId)
        {
            
            ViewBag.EpisodeId = episodeId;

            return PartialView("_ResultsArchive");
        }

        /// <summary>
        /// Retorna a planiha de resultados para ser preenchida
        /// </summary>
        /// <returns></returns>
        [Route("baixarPlanilha/{episodeId}")]
        [HttpGet]
        public FileContentResult DownloadPlan(string episodeId)
        {

            List<MetricEngineDTO> metrics = MetricEngineService.Instance.GetByGameId(CurrentFirm.ExternalId).List.metric;

            var workbook = new Workbook();

            var worksheetResults = workbook.Worksheets[0];

            rowsCount = 500;

            worksheetResults.Cells.HideColumns(5, 16384);
            worksheetResults.Cells.HideRows(rowsCount, 1048576);
            worksheetResults.Cells.StandardWidth = 35.0;

            worksheetResults.Name = "Results";

            var cellsResults = worksheetResults.Cells;

            cellsResults["A1"].PutValue("Email");
            cellsResults["B1"].PutValue("Métrica");
            cellsResults["C1"].PutValue("Período");
            cellsResults["D1"].PutValue("Resultado");
            cellsResults["E1"].PutValue("Equipe");

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

            var validationPeriod = validations[validations.Add()];
            validationPeriod.Type = ValidationType.Date;
            validationPeriod.Operator = OperatorType.Between;
            DateTime firstDate = DateTime.MinValue;
            validationPeriod.Formula1 = firstDate.AddYears(1899).ToString().Split(' ')[0];
            validationPeriod.Formula2 = DateTime.Now.ToString().Split(' ')[0];
            validationPeriod.InCellDropDown = false;
            validationPeriod.ShowError = true;
            validationPeriod.AlertStyle = ValidationAlertType.Stop;
            CellArea areaPeriod;
            areaPeriod.StartRow = 1;
            areaPeriod.EndRow = rowsCount;
            areaPeriod.StartColumn = 2;
            areaPeriod.EndColumn = 2;
            validationPeriod.AreaList.Add(areaPeriod);

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
            areaTeam.StartColumn = 4;
            areaTeam.EndColumn = 4;
            validationTeam.AreaList.Add(areaTeam);

            MemoryStream ms = new MemoryStream();

            ms = workbook.SaveToStream();
            

            return File(ms.ToArray(), "application/vnd.ms-excel");
        }

        /// <summary>
        /// Salva as informações do resultado via arquivo
        /// </summary>
        /// <param name="resultsArchive"></param>
        /// <returns></returns>
        [Route("salvarResultadoArquivo")]
        [HttpPost]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE")]
        public ActionResult SaveResultArchive(HttpPostedFileBase resultsArchive, string teste_12_14)
        {
            try
            {
                string episodeId = Request["episodeId"];

                resultsArchive.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), resultsArchive.FileName));

                string path = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(resultsArchive.FileName));

                var archive = new ExcelQueryFactory(path);

                var rows = from x in archive.WorksheetRange("A1", "E" + rowsCount, "Results")
                           select x;

                foreach (var row in rows)
                {
                    if(row[0] == null || row[0].ToString().Equals("") || row[3].ToString().Equals("0")) {
                        continue;
                    }

                    RunMetricEngineDTO result = new RunMetricEngineDTO();

                    MetricEngineDTO metric = MetricEngineService.Instance.GetDTOByGameAndName(CurrentFirm.ExternalId, row[1].ToString());

                    if (metric == null)
                    {
                        continue;
                    }

                    UserProfileEntity user = UserProfileRepository.Instance.GetByEmail(row[0].ToString());

                    if (user == null)
                    {
                        continue;
                    }

                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId((int)user.Id);

                    if (worker == null)
                    {
                        continue;
                    }

                    TeamEngineDTO team = TeamEngineService.Instance.GetByEpisodeIdAndNick(episodeId,row[4]);

                    if (team == null)
                    {
                        continue;
                    }

                    RunEngineDTO run = RunEngineService.Instance.GetRunByPlayerAndTeamId(worker.ExternalId, team.Id);

                    if(run == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(row[0].ToString()) && !string.IsNullOrWhiteSpace(row[1].ToString()) && !string.IsNullOrWhiteSpace(row[2].ToString()) && !string.IsNullOrWhiteSpace(row[3].ToString()))
                    {
                        result.Ceiling = metric.Ceiling;
                        result.Date = Convert.ToDateTime(row[2].ToString()).Ticks;
                        result.Description = metric.Description;
                        result.Floor = metric.Floor;
                        result.MetricId = metric.Id;
                        result.Multiplier = metric.Multiplier;
                        result.Name = metric.Name;
                        result.Points = int.Parse(row[3].ToString());
                        result.Score = 0;
                        result.Xp = metric.Xp;
                        result.RunId = run.Id;
                        result.PlayerId = worker.ExternalId;

                        RunMetricEngineService.Instance.CreateOrUpdate(result);
                    }
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                //ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar os resultados.");

                return Json(new { Success = false, Exception = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

    }
}
