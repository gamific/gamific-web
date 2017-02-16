using System.Web.Mvc;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/perguntasVideo")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA")]
    public class VideoQuestionController : BaseController
    {
        // GET: About
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

    }
}