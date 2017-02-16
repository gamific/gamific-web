using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;


namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/funcaoMetrica")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class WorkerTypeMetricController : BaseController
    {
        // GET: WorkerTypeMetric
        [Route("associar/{metricId}")]
        public ActionResult Index(string metricId)
        {            
            MetricEngineDTO metric = MetricEngineService.Instance.GetById(metricId);

            ViewBag.MetricId = metricId;
            ViewBag.MetricName = metric.Name;
            ViewBag.Icon = "fa " + metric.Icon.Replace("_","-");

            ViewBag.NumberOfFunctions = WorkerTypeMetricRepository.Instance.GetCountFromMetric(metricId);
            ViewBag.NumberOfFunctionsToAssociate = WorkerTypeMetricRepository.Instance.GetCountToAssociateFromMetric(metricId, CurrentFirm.ExternalId);

            return View();
        }

        [Route("search/{metricId}/{numberOfFunctions:int}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, string metricId, int numberOfFunctions)
        {
            if (jqueryTableRequest != null)
            {
                List<WorkerTypeMetricDTO> searchResult = null;
                searchResult = WorkerTypeMetricRepository.Instance.GetAllDTOFromMetric(metricId, jqueryTableRequest.Page, 10);

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
                        RecordsTotal = numberOfFunctions,
                        RecordsFiltered = numberOfFunctions,
                        Data = searchResult.Select(r => new string[] { r.WorkerTypeName, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = numberOfFunctions,
                        RecordsFiltered = numberOfFunctions,
                        Data = searchResult.Select(r => new string[] { r.WorkerTypeName, r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("searchToAssociate/{metricId}/{NumberOfFunctionsToAssociate:int}")]
        public ActionResult SearchWorkeTypesToAssociate(JQueryDataTableRequest jqueryTableRequest, string metricId, int NumberOfFunctionsToAssociate)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];

                List<WorkerTypeEntity> searchResult = null;
                searchResult = WorkerTypeMetricRepository.Instance.GetAllToAssociate(metricId, CurrentFirm.ExternalId, jqueryTableRequest.Page, 10);

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
                        RecordsTotal = NumberOfFunctionsToAssociate,
                        RecordsFiltered = NumberOfFunctionsToAssociate,
                        Data = searchResult.Select(r => new string[] { r.Id.ToString(), r.TypeName }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = NumberOfFunctionsToAssociate,
                        RecordsFiltered = NumberOfFunctionsToAssociate,
                        Data = searchResult.Select(r => new string[] { r.Id.ToString(), r.TypeName }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("novaAssociacao/{metricId}")]
        public ActionResult Associate(string metricId)
        {
            ViewBag.MetricId = metricId;

            return PartialView("_AssociateWorkerTypes");
        }

        /// <summary>
        /// Salva as associacoes de equipes e jogadores
        /// </summary>
        /// <param name="workerTypesId"></param>
        /// <param name="metricId"></param>
        /// <returns></returns>
        [Route("salvarAssociacoes")]
        [HttpPost]
        public ActionResult SaveAssociations(string workerTypesId, string metricId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    List<string> workerTypesIdList = workerTypesId.Split(',').ToList();

                    foreach (string workerTypeId in workerTypesIdList)
                    {
                        if (!string.IsNullOrWhiteSpace(workerTypeId))
                        {
                            WorkerTypeMetricEntity workerTypeMetric = new WorkerTypeMetricEntity()
                            {
                                UpdatedBy = CurrentUserId,
                                Status = GenericStatus.ACTIVE,
                                MetricExternalId = metricId,
                                WorkerTypeId = int.Parse(workerTypeId)
                            };

                            WorkerTypeMetricRepository.Instance.CreateWorkerTypeMetric(workerTypeMetric);
                        }
                    }

                    Success("Associação feita com sucesso.");
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Error("Ocorreu um erro ao tentar associar um tipo de jogador a essa metrica.", ex);
            }

            return Redirect("/admin/funcaoMetrica/associar/" + metricId);
        }

        [Route("remover/{workerTypeMetricId:int}")]
        public ActionResult Remove(int workerTypeMetricId)
        {
            WorkerTypeMetricEntity workerTypeMetric = WorkerTypeMetricRepository.Instance.GetById(workerTypeMetricId);

            workerTypeMetric.Status = GenericStatus.INACTIVE;

            WorkerTypeMetricRepository.Instance.UpdateWorkerTypeMetric(workerTypeMetric);

            return Redirect("/admin/funcaoMetrica/associar/" + workerTypeMetric.MetricExternalId);
        }
    }
}