using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                Text = i.ToString().Replace("_", "-"),
                Value = i.ToString().Replace("_", "-")
            }).ToList();

            return PartialView("_edit", metric);
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            MetricEngineDTO metric = new MetricEngineDTO();

            ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Selected = metric.Icon == i.ToString().Replace("_", "-") ? true : false,
                Text = i.ToString().Replace("_", "-"),
                Value = i.ToString().Replace("_", "-")
            }).ToList();

            return PartialView("_Edit", metric);
        }

        [Route("remover/{metricId}")]
        public ActionResult Remove(string metricId)
        {
            MetricEngineService.Instance.DeleteByIdAndActiveIsTrue(metricId);
            //MetricEngineService.Instance.DeleteById(metricId);

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
        public ActionResult Save(MetricEngineDTO metric)
        {
            try
            {
                if(metric.GameId == "" || metric.GameId == null)
                {
                    metric.GameId = CurrentFirm.ExternalId;
                }

                ValidateModel(metric);

                MetricEngineDTO newMetric = MetricEngineService.Instance.CreateOrUpdate(metric);

                if(newMetric == null)
                {
                    throw new Exception(".");
                }

                Success("Metrica atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Error("Erro ao atualizar metrica" + ex.Message);
            }

            return new EmptyResult();
        }

        private void ValidateModel(MetricEngineDTO metric)
        {
            List<string> notFilled = new List<string>();

            if (metric.Icon == "" || metric.Icon == null)
            {
                notFilled.Add("icone");
            }
            if (metric.Multiplier == 0 || metric.Multiplier == null)
            {
                notFilled.Add("peso");
            }
            if (metric.Name == "" || metric.Name == null)
            {
                notFilled.Add("nome");
            }
            if (metric.GameId == "" || metric.GameId == null)
            {
                notFilled.Add("Firma");
            }

            if(notFilled.Count != 0)
            {
                string fields = ", campos não preenchidos: ";
                int i;

                for (i = 0; i < notFilled.Count - 1; i++)
                {
                    fields += notFilled[i] + ", ";
                }

                fields += notFilled[i] + ".";

                throw new Exception(fields);
            }
        }
    }
}
