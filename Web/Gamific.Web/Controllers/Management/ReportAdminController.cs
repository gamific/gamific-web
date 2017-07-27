using System;
using System.Transactions;
using System.Web.Mvc;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Model.Account.DTO;
using System.Collections.Generic;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Account;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Util.Instrumentation;
using System.Linq;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;
using System.Web;
using System.IO;
using System.Drawing;
using Vlast.Gamific.Model.Media.Repository;
using Vlast.Gamific.Model.Media.Domain;
using Vlast.Gamific.Account.Model;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Model.Account.Repository;
using Newtonsoft.Json;
using Vlast.Gamific.Model.Firm.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/relatorio")]
    public class ReportAdminController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarEmpresa")]
        [HttpGet]
        public ActionResult SearchEpisodes()
        {
            List<DataEntity> all = DataRepository.Instance.GetAllOfGameIdAndGameName();

            return Json(JsonConvert.SerializeObject(all), JsonRequestBehavior.AllowGet);
        }

        

        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarUsuario/{initDateMonth}/{initDateDay}/{initDateYear}/{finishDateMonth}/{finishDateDay}/{finishDateYear}/{gameId}/{active}")]
        [HttpGet]
        public ActionResult SearchGameDTO(string initDateMonth, string initDateDay, string initDateYear, string finishDateMonth, string finishDateDay, string finishDateYear, string gameId, bool active)
        {
            DateTime initDate = DateTime.Parse(initDateYear + "-" + initDateMonth + "-" + initDateDay + " 00:00:00");

            DateTime finishDate = DateTime.Parse(finishDateYear + "-" + finishDateMonth + "-" + finishDateDay + " 00:00:00");

            List<ReportDTO> workers = null;

            if (active)
            {
                workers = WorkerRepository.Instance.GetWorkerDTOByDate(initDate, finishDate, gameId == "empty" ? "" : gameId);
            }
            else
            {
                workers = WorkerRepository.Instance.GetWorkerDTOByDateAndInative(initDate, finishDate, gameId == "empty" ? "" : gameId);
            }



            return Json(JsonConvert.SerializeObject(workers), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarUsuarioInativo/{initDateMonth}/{initDateDay}/{initDateYear}/{finishDateMonth}/{finishDateDay}/{finishDateYear}/{gameId}")]
        [HttpGet]
        public ActionResult SearchGameDTOInativo(string initDateMonth, string initDateDay, string initDateYear, string finishDateMonth, string finishDateDay, string finishDateYear, string gameId)
        {
            DateTime initDate = DateTime.Parse(initDateYear + "-" + initDateMonth + "-" + initDateDay + " 00:00:00");

            DateTime finishDate = DateTime.Parse(finishDateYear + "-" + finishDateMonth + "-" + finishDateDay + " 00:00:00");

            List<ReportDTO> workers = WorkerRepository.Instance.GetWorkerDTOByDateAndInative(initDate, finishDate, gameId == "empty" ? "" : gameId);
            //List<ReportDTO> workers = WorkerRepository.Instance.GetWorkerDTOByDate(initDate, finishDate, gameId == "empty" ? "" : gameId);
            return Json(JsonConvert.SerializeObject(workers), JsonRequestBehavior.AllowGet);
        }


    }
}