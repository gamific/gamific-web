using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;


namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/metricas")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class MetricController : BaseController
    {
        // GET: Metric
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("editar/{metricId}")]
        public ActionResult Edit(string metricId)
        {
            MetricEngineDTO metric = MetricEngineService.Instance.GetById(metricId);

            ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Selected = metric.Icon == i.ToString().Replace("_", "-") ? true : false,
                Text = i.GetType().GetMember(i.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name,
                Value = i.ToString().Replace("_", "-")
            }).ToList();

            ViewBag.WorkerTypes = WorkerTypeRepository.Instance.GetAllByGameId(CurrentFirm.ExternalId);

            ViewBag.WorkerTypeMetrics = WorkerTypeMetricRepository.Instance.GetAllWorkerTypesByMetricId(metricId).Select(x => x.WorkerTypeId).ToList();

            return PartialView("_edit", metric);
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            MetricEngineDTO metric = new MetricEngineDTO();

            ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Selected = metric.Icon == i.ToString().Replace("_", "-") ? true : false,
                Text = i.GetType().GetMember(i.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name,
                Value = i.ToString().Replace("_", "-")
            }).ToList();

            ViewBag.WorkerTypes = WorkerTypeRepository.Instance.GetAllByGameId(CurrentFirm.ExternalId);

            return PartialView("_Edit", metric);
        }

        [Route("remover/{metricId}")]
        public ActionResult Remove(string metricId)
        {
            //MetricEngineService.Instance.DeleteByIdAndActiveIsTrue(metricId);
            MetricEngineService.Instance.DeleteById(metricId);

            return View("Index");
        }

        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
        {

            int index = 0;
            if (jqueryTableRequest.Order != null)
            {
                index = Int32.Parse(jqueryTableRequest.Order);
            }

            if (jqueryTableRequest != null)
            {
                GetAllDTO all = MetricEngineService.Instance.GetAllDTOByGame(CurrentFirm.ExternalId, jqueryTableRequest.Page);

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.metric.Select(r => new string[] { r.Icon + ";" + r.Name, r.Multiplier.ToString(), r.Id }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.metric.Select(r => new string[] { r.Icon + ";" + r.Name, r.Multiplier.ToString(), r.Id }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(MetricEngineDTO metric, List<CheckBoxValue> checkBoxes)
        {
            try
            {
                if(metric.GameId == "" || metric.GameId == null)
                {
                    metric.GameId = CurrentFirm.ExternalId;
                }
              
                if (ModelState.IsValid)
                {

                
                ValidateModel(metric);

                MetricEngineDTO newMetric = MetricEngineService.Instance.CreateOrUpdate(metric);

                List<int> workerTypeMetrics = WorkerTypeMetricRepository.Instance.GetAllWorkerTypesByMetricId(newMetric.Id).Select(x => x.WorkerTypeId).ToList();

                foreach(CheckBoxValue checkBox in checkBoxes)
                {
                    if(checkBox.Checked && !workerTypeMetrics.Contains(checkBox.Value))
                    {
                        WorkerTypeMetricEntity wtm = 
                        WorkerTypeMetricRepository.Instance.CreateWorkerTypeMetric(new WorkerTypeMetricEntity
                        {
                            MetricExternalId = newMetric.Id,
                            Status = GenericStatus.ACTIVE,
                            UpdatedBy = CurrentUserId,
                            WorkerTypeId = checkBox.Value
                        });
                    }
                    else if(!checkBox.Checked && workerTypeMetrics.Contains(checkBox.Value))
                    {
                        WorkerTypeMetricEntity toRemove = WorkerTypeMetricRepository.Instance.GetByWorkerTypeIdAndMetricId(checkBox.Value, newMetric.Id);
                        WorkerTypeMetricRepository.Instance.Remove(toRemove);
                    }
                }

                if(newMetric == null)
                {
                    throw new Exception(".");
                }

                Success("Metrica atualizada com sucesso.");
                }
                else
                {
                    ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
                    {
                        Selected = metric.Icon == i.ToString().Replace("_", "-") ? true : false,
                        Text = i.GetType().GetMember(i.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name,
                        Value = i.ToString().Replace("_", "-")
                    }).ToList();

                    ViewBag.WorkerTypes = WorkerTypeRepository.Instance.GetAllByGameId(CurrentFirm.ExternalId);

                    ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar a Métrica.");

                    return PartialView("_Edit", metric);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Error("Erro ao atualizar metrica" + ex.Message);
            }

            return new EmptyResult();
        }

        
    }
}
