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
    [RoutePrefix("admin/episode")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class EpisodeController : BaseController
    {
        // GET: Episode
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("editar/{episodeId}")]
        public ActionResult Edit(string episodeId)
        {
            EpisodeEngineDTO episode = EpisodeEngineService.Instance.GetById(episodeId);

            if (episode.Active.Equals(0) || episode.Active == false)
            {
                //episode.Active = true;
            }
            else
            {
                episode.GameId = CurrentFirm.ExternalId;

                episode.initDateAux = new DateTime(episode.initDate);
                
                episode.finishDateAux = new DateTime(episode.finishDate);
                

                ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                }).ToList();
            }

            if(episode.DaysOfWeek == null || episode.Active == false)
            {
                episode.DaysOfWeek = "";
            }

            return PartialView("_edit", episode);
        }

        [Route("clonar/{episodeId}")]
        public ActionResult Clonar(string episodeId)
        {
            EpisodeEngineDTO episode = EpisodeEngineService.Instance.GetById(episodeId);

           ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Text = i.ToString(),
                Value = i.ToString()
            }).ToList();

            return PartialView("_Clone", episode);
        }

        [Route("newClone")]
        [HttpPost]
        public ActionResult NewClone(String name, String id)
        {
            long initDate = DateTime.Now.Ticks;

            long finishDate = DateTime.Now.Ticks;

            EpisodeEngineDTO episode = EpisodeEngineService.Instance.Clone(name, id, initDate, finishDate);

            return new EmptyResult();
        }

        [Route("clean/{episodeId}")]
        public ActionResult Cleaning(string episodeId)
        {
            //List<EpisodeEngineDTO> episode = EpisodeEngineService.Instance.Clean(episodeId);
            EpisodeEngineService.Instance.DeleteAllScoreByEpisodeId(episodeId);

            return new EmptyResult();
        }




        [Route("cadastrar")]
        public ActionResult Create()
        {
            EpisodeEngineDTO episode = new EpisodeEngineDTO();
            episode.Active = true;
            
            ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
            {
                Text = i.ToString(),
                Value = i.ToString()
            }).ToList();

            return PartialView("_Edit", episode);
        }


        [Route("remover/{episodeId}")]
        public ActionResult Remove(string episodeId)
        {
      
            EpisodeEngineService.Instance.CloseById(episodeId);
           
            return View("Index");
        }


        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(EpisodeEngineDTO episode)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if(episode.Active == false)
                    {
                        Success("Campanha não pode ser alterada.");
                    }
                    else
                    {
                        if (episode.Id == null || episode.Id.Equals(""))
                        {
                            episode.Active = true;
                        }

                        
                        episode.initDate = episode.initDateAux.Ticks;

                        episode.finishDate = episode.finishDateAux.Ticks;

                        episode.GameId = CurrentFirm.ExternalId;

                        ValidateModel(episode);

                        EpisodeEngineDTO newEpisode = EpisodeEngineService.Instance.CreateOrUpdate(episode);

                        if (newEpisode == null)
                        {
                            throw new Exception(".");
                        }

                        Success("Campanha atualizada com sucesso.");
                    }


                   

                }
                else
                {
                   
                    episode.Active = true;
                    ViewBag.Icons = Enum.GetValues(typeof(Icons)).Cast<Icons>().Select(i => new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    }).ToList();
                    ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar a Campanha.");
                    return PartialView("_Edit", episode);
                   
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                Error("Erro ao atualizar campanha" + ex.Message);
                return PartialView("_Edit");
            }

            return  new EmptyResult();
        }


        [Route("searchAssociate/{id}")]
        public ActionResult SearchAssociate(JQueryDataTableRequest jqueryTableRequest, int id)
        {

            int index = jqueryTableRequest.Page;

            var listAssociated = QuizCampaignService.Instance.getByAssociated(id);
            var episodeList = new List<EpisodeEngineDTO>();
            foreach (var item in listAssociated)
            {
                var episode = EpisodeEngineService.Instance.GetById(item.IdCampaign);
                episode.Id = item.Id.ToString();
                episodeList.Add(episode);
            }
            DateTime dateInit;
            DateTime dateFinish;

            if (jqueryTableRequest != null)
            {

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsFiltered = episodeList.Count(),
                        Data = episodeList.Select(r => new string[] { r.Id, r.Name, (dateInit = new DateTime(r.initDate)).ToString("dd/MM/yyyy"), (dateFinish = new DateTime(r.finishDate)).ToString("dd/MM/yyyy"), r.XpReward.ToString(), r.Active == true ? "Sim" : "Não", r.sendEmail == true ? "Sim" : "Não", r.Id }).ToArray().ToArray() //.OrderBy(item => item[index]).ToArray()

                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
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
                GetAllDTO all = EpisodeEngineService.Instance.GetByGameId(CurrentFirm.ExternalId, jqueryTableRequest.Page);
                JQueryDataTableResponse response = null;
                DateTime dateInit;
                DateTime dateFinish;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.episode.Select(r => new string[] { r.Name, (dateInit = new DateTime(r.initDate)).ToString("dd/MM/yyyy"), (dateFinish = new DateTime(r.finishDate)).ToString("dd/MM/yyyy"), r.XpReward.ToString(), r.Active == true ? "Sim" : "Não", r.sendEmail == true ? "Sim" : "Não", r.Id }).ToArray().ToArray() //.OrderBy(item => item[index]).ToArray()

                    };

                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.episode.Select(r => new string[] { r.Name, (dateInit = new DateTime(r.initDate)).ToString("dd/MM/yyyy"), (dateFinish = new DateTime(r.finishDate)).ToString("dd/MM/yyyy"), r.XpReward.ToString(), r.Active == true ? "Sim" : "Não", r.sendEmail == true ? "Sim" : "Não", r.Id }).ToArray().OrderByDescending(item => item[index]).ToArray()

                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);


        }

        [Route("associate/remover/{id}")]
        public ActionResult RemoveAssociate(int id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    QuizCampaignService.Instance.delete(id);
                    scope.Complete();

                    return Json(new { status = "sucess", message = "Registro removido com sucesso!" });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { status = "error", message = "Ocorreu um problema!" });
            }


        }

        [Route("searchWithID")]
        public ActionResult searchWithID(JQueryDataTableRequest jqueryTableRequest)
        {
            int index = 0;
            if (jqueryTableRequest.Order != null)
            {
                index = Int32.Parse(jqueryTableRequest.Order);
            }
            if (jqueryTableRequest != null)
            {
                GetAllDTO all = EpisodeEngineService.Instance.GetByGameId(CurrentFirm.ExternalId, jqueryTableRequest.Page);
                JQueryDataTableResponse response = null;
                DateTime dateInit;
                DateTime dateFinish;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.episode.Select(r => new string[] {r.Id, r.Name, (dateInit = new DateTime(r.initDate)).ToString("dd/MM/yyyy"), (dateFinish = new DateTime(r.finishDate)).ToString("dd/MM/yyyy"), r.XpReward.ToString(), r.Active == true ? "Sim" : "Não", r.sendEmail == true ? "Sim" : "Não", r.Id }).ToArray().ToArray() //.OrderBy(item => item[index]).ToArray()

                    };

                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = all.PageInfo.totalElements,
                        RecordsFiltered = all.PageInfo.totalElements,
                        Data = all.List.episode.Select(r => new string[] { r.Id, r.Name, (dateInit = new DateTime(r.initDate)).ToString("dd/MM/yyyy"), (dateFinish = new DateTime(r.finishDate)).ToString("dd/MM/yyyy"), r.XpReward.ToString(), r.Active == true ? "Sim" : "Não", r.sendEmail == true ? "Sim" : "Não", r.Id }).ToArray().OrderByDescending(item => item[index]).ToArray()

                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}