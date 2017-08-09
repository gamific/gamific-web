using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/implantacao")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class ImplantationController : Controller
    {
        // GET: Implantation
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}