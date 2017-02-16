using System.Web.Mvc;

namespace Vlast.Gamific.Web.Controllers.Public
{
    [CustomAuthorize]
    [RoutePrefix("public/sobre")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRATOR,ADMINISTRADOR,LIDER,JOGADOR")]
    public class AboutController : BaseController
    {
        // GET: About
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

    }
}