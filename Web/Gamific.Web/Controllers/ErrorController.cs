using System.Net;
using System.Web.Mvc;

namespace Vlast.Gamific.Web.Controllers
{
    /// <summary>
    /// Tratamento de erros
    /// </summary>
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Http404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("Error");
        }

        public ViewResult Http403()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View("Error");
        }
    }
}