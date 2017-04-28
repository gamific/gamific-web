using Newtonsoft.Json;
using System.Web.Mvc;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/notificacoes")]
    [CustomAuthorize(Roles = "ADMINISTRADOR")]
    public class PushNotificationController : BaseController
    {
        // GET: PushNotification
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Busca os episodios
        /// </summary>
        /// <returns></returns>
        [Route("buscarEpisodios")]
        [HttpGet]
        public ActionResult SearchEpisodes(int state = 1)
        {
            GetAllDTO all = EpisodeEngineService.Instance.GetByGameIdAndActive(CurrentFirm.ExternalId, state);

            return Json(JsonConvert.SerializeObject(all.List.episode), JsonRequestBehavior.AllowGet);
        }
    }
}