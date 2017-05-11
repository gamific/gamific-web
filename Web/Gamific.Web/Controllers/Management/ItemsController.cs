using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [RoutePrefix("admin/items")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class ItemsController : BaseController
    {
        // GET: Items
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Abre a tela de cadastro de um item
        /// </summary>
        /// <returns></returns>
        [Route("cadastrar")]
        public ActionResult Create()
        {
            ItemEngineDTO items = new ItemEngineDTO();

            ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Text = i.ToString(),
                Value = i.ToString()
            }).ToList();

            return PartialView("_Edit", items);
        }

        [Route("remover/{itemId}")]
        public ActionResult Remove(string itemId)
        {

            ItemEngineService.Instance.CloseById(itemId);

            return View("Index");
        }


        [Route("editar/{itemId}")]
        public ActionResult Edit(string itemId)
        {
            ItemEngineDTO item = ItemEngineService.Instance.GetById(itemId);

            
                item.GameId = CurrentFirm.ExternalId;
            
                ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                }).ToList();
            

            return PartialView("_edit", item);
        }


        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(ItemEngineDTO items)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(items.Name == null || items.Name == "")
                    {
                        Error("O item deve possuir um nome.");
                    }
                    else
                    {
                        items.GameId = CurrentFirm.ExternalId;

                        ValidateModel(items);

                        ItemEngineDTO newItems = ItemEngineService.Instance.CreateOrUpdate(items);

                        if (newItems == null)
                        {
                            throw new Exception(".");
                        }

                        Success("Item atualizado com sucesso.");
                    }
                    
                    
                    
                }
                else
                {
                    ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar a Função.");
                    return PartialView("_Edit");

                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Error("Erro ao atualizar Item" + ex.Message);
                return PartialView("_Edit");
            }

            return new EmptyResult();
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
                GetAllDTO all = ItemEngineService.Instance.GetByGameId(CurrentFirm.ExternalId, jqueryTableRequest.Page);
                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.item.Select(r => new string[] { r.Name, r.Id }).ToArray().ToArray() //.OrderBy(item => item[index]).ToArray()

                    };

                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.item.Select(r => new string[] { r.Name, r.Id }).ToArray().OrderByDescending(item => item[index]).ToArray()

                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}