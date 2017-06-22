using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Public.Model;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.BIZ;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Public
{
    [CustomAuthorize]
    [RoutePrefix("public/hierarchy")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,LIDER,JOGADOR")]
    public class HierarchyController : BaseController
    {

        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, 1);
            ViewBag.Episodes = from episode in all.List.episode
                               select new SelectListItem
                               {
                                   Value = episode.Id.ToString(),
                                   Text = episode.Name
                               };


            ViewBag.EpisodeId = all.List.episode.Count > 0 ? all.List.episode[0].Id : "0" ;

            return View("");
        }

        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("searchHierarchy")]
        [HttpGet]
        public ActionResult searchHierarchy(string episodeId)
        {
            string ret = TeamEngineService.Instance.getHierarchy(episodeId);

            return Content(ret, "application/json");
           // return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
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

    }
}