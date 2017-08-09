using System.Collections.Generic;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Public.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Controllers.Public.Model;
using System.Linq;
using Newtonsoft.Json;
using System;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Account.Model;
using Vlast.Gamific.Model.Account.Domain;


namespace Vlast.Gamific.Web.Controllers.Public
{
    [RoutePrefix("public/hallDaFama")]
    [CustomAuthorize(Roles = "WORKER,LIDER,JOGADOR")]
    public class HallOfFameController : BaseController
    {
        // GET: HallOfFame
        [Route("")]
        public ActionResult Index(int state = 1)
        {
            
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndInactive(CurrentFirm.ExternalId);
            ViewBag.Episodes = from episode in all.List.episode
                               select new SelectListItem
                               {
                                   Value = episode.Id.ToString(),
                                   Text = episode.Name
                               };
            /*
            List<PlayerEngineDTO> players = new List<PlayerEngineDTO>();
            foreach(EpisodeEngineDTO one in all.List.episode)
            {
                players.AddRange(HallOfFameEngineService.Instance.GetByEpisodeId(one.Id).List.player);
            }
            List<SelectListItem> lista = players.Select(player => new SelectListItem { Value = player.Id.ToString(), Text = player.Nick }).ToList();*/

            //teste
            GetAllDTO halls = HallOfFameEngineService.Instance.GetByGameId(CurrentFirm.ExternalId);
            /*ViewBag.AllPlayers = from hall in halls.List.hallOfFame
                                 select new SelectListItem
                                 {
                                     Group = new SelectListGroup(),
                                     Value = hall.GeneralWinners[0].logoPath,
                                     Text = "1º" + hall.GeneralWinners[0].PlayerName + " " +"Campanha:" + hall.EpisodeName  
                                 };*/
            if(halls.List.hallOfFame.Count != 0)
            { 
            ViewBag.AllPlayers = from hall in halls.List.hallOfFame
                                 select new HallOfFameEngineDTO
                                 {
                                     GeneralWinners = hall.GeneralWinners,
                                     EpisodeName = hall.EpisodeName
                                 };
            }
            else
            {
                ViewBag.AllPlayers = null;
            }
            //-----------
            //ViewBag.AllPlayers = hall.List.hallOfFame.FirstOrDefault();


            return View("Index");
        }

        /// <summary>
        /// Carrega um datatable com todas as equipes do episodio ordenados por Score
        /// </summary>
        /// <param name="jqueryTableRequest"></param>
        /// <param name="episodeId"></param>
        /// <param name="metricId"></param>
        /// <returns></returns>
        [Route("searchByGameId")]
        public ActionResult SearchPlayers(JQueryDataTableRequest jqueryTableRequest, string gameId)
        {
            /* GetAllDTO all;

             if (jqueryTableRequest != null)
             {

                 all = RunEngineService.Instance.ScoreByEpisodeIdAndMetricId(episodeId, metricId == "empty" ? "" : metricId, jqueryTableRequest.Page);

                 foreach (RunEngineDTO run in all.List.run)
                 {
                     try
                     {
                         run.LogoId = PlayerEngineService.Instance.GetById(run.PlayerId).LogoId;
                     }
                     catch (Exception e)
                     {
                         run.LogoId = 0;
                     }
                 }


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
                         RecordsTotal = all.PageInfo.totalElements,
                         RecordsFiltered = all.PageInfo.totalElements,
                         Data = all.List.run.Select(t => new string[] { jqueryTableRequest.Page.ToString(), t.PlayerName + ";" + t.LogoId + ";" + t.PlayerId + ";" + t.TeamId, t.TeamName, t.Score.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                     };
                 }
                 else
                 {
                     response = new JQueryDataTableResponse()
                     {
                         Draw = jqueryTableRequest.Draw,
                         RecordsTotal = all.PageInfo.totalElements,
                         RecordsFiltered = all.PageInfo.totalElements,
                         Data = all.List.run.Select(t => new string[] { jqueryTableRequest.Page.ToString(), t.PlayerName + ";" + t.LogoId + ";" + t.PlayerId + ";" + t.TeamId, t.TeamName, t.Score.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                     };
                 }

                 return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
             }

             return Json(null, JsonRequestBehavior.AllowGet);*/
            return null;
        }


        [Route("search/{playerId}")]
        public ActionResult Search(string playerId)//(JQueryDataTableRequest jqueryTableRequest, int idPlayer)
        {
            GetAllDTO halls = HallOfFameEngineService.Instance.GetByGameId(CurrentFirm.ExternalId);
            HallOfFameEngineDTO playerSelected;

            foreach (HallOfFameEngineDTO hall in halls.List.hallOfFame)
            {

                if (hall.GeneralWinners[0].PlayerId == playerId.ToString())
                {
                    playerSelected = hall;
                    break;
                }
            }
            // return Json(JsonConvert.SerializeObject(playerSelected), JsonRequestBehavior.AllowGet);
            return Json(JsonConvert.SerializeObject(""), JsonRequestBehavior.AllowGet);

        }

        [Route("remover/{episodeId}")]
        public ActionResult Remove(string episodeId)
        {
            HallOfFameEngineService.Instance.DeleteByEpisodeId(episodeId);

            return View("Index");
        }

    }
}