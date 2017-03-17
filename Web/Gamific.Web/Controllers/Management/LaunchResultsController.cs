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
using System.Diagnostics;
using Vlast.Broker.EMAIL;
using Vlast.Util.Parameter;

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

            float valorVendas = 0f;
            if (CurrentFirm.ExternalId == "5880a1743a87783b4f0ba709")
            {
                valorVendas = resultList.Where(x => x.Name == "VENDAS").Select(y => y.Points).FirstOrDefault();
            }
            

            foreach (RunMetricEngineDTO result in resultList)
            {
                if (result.Points > 0)
                {
                    result.Score = 0;
                    result.RunId = runId;
                    result.Date = time;
                    result.ArithmeticMultiplier = valorVendas > 0 ? valorVendas : 1;
                    RunMetricEngineService.Instance.CreateOrUpdate(result);
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        private void RefreshWorkersOnTeamCache(string teamId)
        {
            HttpRuntime.Cache.Remove("WorkerExternalIdsOnTeam");

            TeamEngineDTO team = TeamEngineService.Instance.GetById(teamId);
            GetAllDTO all = RunEngineService.Instance.GetRunsByTeamId(teamId);

            if (all != null)
            {
                List<string> externalWorkerIds = (from r in all.List.run select r.PlayerId).Where(q => q != team.MasterPlayerId).ToList();
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

            rowsCount = 5000;

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

        [Route("salvarResultadoArquivo")]
        [HttpPost]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE")]
        public ActionResult SaveResultArchive(HttpPostedFileBase resultsArchive, string episodeId)
        {
            if(CurrentFirm.ExternalId == "5885f7593a87786bec6ca6fd")
            {
                return SaveResultArchiveSolBebidas(resultsArchive, episodeId);
            }
            else
            {
                return SaveResultArchiveStandard(resultsArchive, episodeId);
            }
        }

        
        //Sol bebidas
        /// <summary>
        /// Salva as informações do resultado via arquivo
        /// </summary>
        /// <param name="resultsArchive"></param>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        //[Route("salvarResultadoArquivo")]
        //[HttpPost]
        //[CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE")]
        public ActionResult SaveResultArchiveSolBebidas(HttpPostedFileBase resultsArchive, string episodeId)
        {
            int ANO = 0;
            int MES = 1;
            int REPRESENTANTE = 2;
            int EMAIL = 3;
            int CLIENTE = 4;
            int COD_PRODUTO = 5;
            int PRODUTO = 6;
            int QTDE_OBJ = 7;
            int QTDE_REAL = 8;
            int PERCENT_QTDE = 9;
            int VLR_OBJ = 10;
            int VLR_BRUTO_REAL = 11;
            int PERCENT_VALOR = 12;
            int VLR_MEDIO_OBJETIVO = 13;
            int VLR_MEDIO_REAL = 14;

            EpisodeEngineDTO episode = EpisodeEngineService.Instance.GetById(episodeId);
            string gameId = CurrentFirm.ExternalId;

            string errors = "Erros: {0}<br/>";

            List<GoalEngineDTO> goalsTotalFat = new List<GoalEngineDTO>();
            List<GoalEngineDTO> goalsTotalVol = new List<GoalEngineDTO>();

            MetricEngineDTO metricFat;
            MetricEngineDTO metricVol;

            int line = 1;
            int errorsCount = 0;

            try
            {
                metricFat = MetricEngineService.Instance.GetDTOByGameAndName(gameId, "FATURAMENTO");
            }
            catch (Exception e)
            {
                errors += "Metrica (Faturamento) não encontrado.<br/>";
                errorsCount++;
                metricFat = new MetricEngineDTO();
                Debug.Print("Error metric: " + e.Message);
            }

            try
            {
                metricVol = MetricEngineService.Instance.GetDTOByGameAndName(gameId, "VOLUME");
            }
            catch (Exception e)
            {
                errors += "Metrica (Volume) não encontrado.<br/>";
                errorsCount++;
                metricVol = new MetricEngineDTO();
                Debug.Print("Error metric: " + e.Message);
            }

            try
            {
                EpisodeEngineService.Instance.DeleteAllScoreByEpisodeId(episodeId);

                resultsArchive.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), resultsArchive.FileName));

                var archive = new ExcelQueryFactory(Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(resultsArchive.FileName)));

                var rows = from x in archive.WorksheetRange("A1", "N" + rowsCount, "Planilha1") select x;

                float points;

                foreach (var row in rows)
                {
                    line++;

                    PlayerEngineDTO player;
                    RunEngineDTO run;

                    try
                    {
                        player = PlayerEngineService.Instance.GetByEmail(row[EMAIL].ToString().Trim().ToLower());
                    }
                    catch(Exception e)
                    {
                        Debug.Print("Error player: " + e.Message);
                        errors += "(Linha -> " + line + "°, Coluna -> 'Representante') " + "Jogador: " + row[EMAIL].ToString().Trim() + " não encontrado.<br/>";
                        errorsCount++;
                        continue;
                    }

                    try
                    {
                        run = RunEngineService.Instance.GetByEpisodeIdAndPlayerId(episodeId, player.Id);
                    }
                    catch (Exception e)
                    {
                        Debug.Print("Error run: " + e.Message);
                        errors += "(Linha -> " + line + "°, Coluna -> 'Email') " + "Jogador: " + row[EMAIL].ToString().Trim() + " não participa desta campanha.<br/>";
                        errorsCount++;
                        continue;
                    }

                    float.TryParse(row[VLR_OBJ].ToString(), out points);
                    if (goalsTotalFat.Find(x => x.RunId == run.Id) != null)
                    {                      
                        goalsTotalFat.Find(x => x.RunId == run.Id).Goal += points;
                    }
                    else
                    {
                        GoalEngineDTO goalFat;
                        try
                        {
                            goalFat = GoalEngineService.Instance.GetByRunIdAndMetricId(run.Id, metricFat.Id);
                            goalFat.Goal = points;
                        }
                        catch (Exception e)
                        {
                            goalFat = new GoalEngineDTO
                            {
                                Goal = points,
                                MetricIcon = metricFat.Icon,
                                MetricId = metricFat.Id,
                                MetricName = metricFat.Name,
                                Percentage = 0,
                                RunId = run.Id
                            };

                            Debug.Print("Goal faturamento: " + e.Message);
                        }

                        goalsTotalFat.Add(goalFat);
                    }

                    float.TryParse(row[QTDE_OBJ].ToString(), out points);
                    if (goalsTotalVol.Find(x => x.RunId == run.Id) != null)
                    {
                        goalsTotalVol.Find(x => x.RunId == run.Id).Goal += points;
                    }
                    else
                    {
                        GoalEngineDTO goalVol;
                        try
                        {
                            goalVol = GoalEngineService.Instance.GetByRunIdAndMetricId(run.Id, metricVol.Id);
                            goalVol.Goal = points;
                        }
                        catch (Exception e)
                        {
                            goalVol = new GoalEngineDTO
                            {
                                Goal = points,
                                MetricIcon = metricVol.Icon,
                                MetricId = metricVol.Id,
                                MetricName = metricVol.Name,
                                Percentage = 0,
                                RunId = run.Id
                            };

                            Debug.Print("Goal volume: " + e.Message);
                        }

                        goalsTotalVol.Add(goalVol);
                    }

                    ItemEngineDTO item = new ItemEngineDTO
                    {
                        GameId = gameId,
                        Name = row[PRODUTO].ToString().Trim()
                    };

                    try
                    {
                        item = ItemEngineService.Instance.FindByNameAndGameId(item.Name, item.GameId);
                    }
                    catch(Exception e)
                    {
                        item = ItemEngineService.Instance.CreateOrUpdate(item);
                        Debug.Print("Error metric: " + e.Message);
                    }

                    float.TryParse(row[VLR_BRUTO_REAL].ToString(), out points);
                    RunMetricEngineDTO resultFaturamento = new RunMetricEngineDTO
                    {
                        Ceiling = metricFat.Ceiling,
                        Description = metricFat.Description,
                        Floor = metricFat.Floor, 
                        MetricId = metricFat.Id,
                        Multiplier = metricFat.Multiplier,
                        Name = metricFat.Name,
                        Xp = 0,
                        Score = 0,
                        Points = (int)points,
                        Date = DateTime.Now.Ticks,
                        PlayerId = player.Id,
                        ItemId = item.Id,
                        RunId = run.Id
                    };

                    float.TryParse(row[QTDE_REAL].ToString(), out points);
                    RunMetricEngineDTO resultVolume = new RunMetricEngineDTO
                    {
                        Ceiling = metricVol.Ceiling,
                        Description = metricVol.Description,
                        Floor = metricVol.Floor,
                        MetricId = metricVol.Id,
                        Multiplier = metricVol.Multiplier,
                        Name = metricVol.Name,
                        Xp = 0,
                        Score = 0,
                        Points = (int)points,
                        Date = DateTime.Now.Ticks,
                        PlayerId = player.Id,
                        ItemId = item.Id,
                        RunId = run.Id
                    };
                    
                    RunMetricEngineService.Instance.CreateOrUpdate(resultFaturamento);
                    RunMetricEngineService.Instance.CreateOrUpdate(resultVolume);
                }

                foreach(GoalEngineDTO goal in goalsTotalVol)
                {
                    GoalEngineService.Instance.CreateOrUpdate(goal);
                }

                foreach (GoalEngineDTO goal in goalsTotalFat)
                {
                    GoalEngineService.Instance.CreateOrUpdate(goal);
                }

                errors = string.Format(errors, errorsCount);
                string emailFrom = ParameterCache.Get("SUPPORT_EMAIL");
                string subject = errorsCount >= 1 ? "Erros ao subir planilha de resultados" : "O lançamento de resultados foi um sucesso.";
                subject = CurrentFirm.FirmName + ": " + subject;
                bool r = EmailDispatcher.SendEmail(emailFrom, subject, new List<string>() { emailFrom, CurrentUserProfile.Email }, errors);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.Print("Geral Error: " + e.Message);

                return Json(new { Success = false, Exception = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        


        /// <summary>
        /// Salva as informações do resultado via arquivo
        /// </summary>
        /// <param name="resultsArchive"></param>
        /// <returns></returns>
        //[Route("salvarResultadoArquivo")]
        //[HttpPost]
        //[CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE")]
        private ActionResult SaveResultArchiveStandard(HttpPostedFileBase resultsArchive, string episodeId)
        {
            string errors = "Quantidade de erros: {0}<br/>Última linha lida: {1}<br/>";
            int line = 1;
            int countErrors = 0;
            int countEmptyLines = 0;

            try
            {
                resultsArchive.SaveAs(Path.Combine(Server.MapPath("~/App_Data"), resultsArchive.FileName));

                string path = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(resultsArchive.FileName));

                var archive = new ExcelQueryFactory(path);

                var rows = from x in archive.WorksheetRange("A1", "E" + rowsCount, "Results")
                           select x;

                foreach (var row in rows)
                {
                    line++;

                    if(countEmptyLines >= 3)
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

                    RunMetricEngineDTO result = new RunMetricEngineDTO();
                    MetricEngineDTO metric = new MetricEngineDTO();
                    TeamEngineDTO team = new TeamEngineDTO();
                    RunEngineDTO run = new RunEngineDTO();

                    try
                    {
                        metric = MetricEngineService.Instance.GetDTOByGameAndName(CurrentFirm.ExternalId, row[1].ToString());
                    }
                    catch(Exception e)
                    {
                        errors += "Erro na coluna 2 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    UserProfileEntity user = UserProfileRepository.Instance.GetByEmail(row[0].ToString());

                    if (user == null)
                    {
                        errors += "Erro na coluna 1 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId((int)user.Id);

                    if (worker == null)
                    {
                        errors += "Erro na coluna 1 da linha " + line + "<br/>";
                        countErrors++;
                        continue;
                    }

                    try
                    {
                        team = TeamEngineService.Instance.GetByEpisodeIdAndNick(episodeId, row[4]);
                    }
                    catch (Exception e)
                    {
                        errors += "Erro na coluna 5 da linha " + line + "<br/>";
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

                errors = string.Format(errors, countErrors, line);

                string emailFrom = ParameterCache.Get("SUPPORT_EMAIL");
                string subject = countErrors >= 1 ? "Erros ao subir planilha de resultados" : "O lançamento de resultados foi um sucesso.";
                subject = CurrentFirm.FirmName + ": " + subject;
                bool r = EmailDispatcher.SendEmail(emailFrom, subject, new List<string>() { emailFrom, CurrentUserProfile.Email }, errors);

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
