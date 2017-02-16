using System.Collections.Generic;
using System.Web.Mvc;
using Vlast.Broker.EMAIL;
using Vlast.Gamific.Web.Controllers.Management.Model;
using Vlast.Util.Parameter;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/suporte")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,LIDER,JOGADOR")]
    public class SupportController : BaseController
    {
        // GET: Support
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Categories = GetCategories();

            return View();
        }

        [Route("enviar")]
        [HttpPost]
        public ActionResult Send(EmailSupportDTO email)
        {
            string emailTo = ParameterCache.Get("SUPPORT_EMAIL");

            bool result = EmailDispatcher.SendEmail(emailTo, email.Subject, new List<string>() { emailTo }, email.Category + " - " + email.Msg);

            if (result)
            {
                Success("Email enviado com sucesso.");
            }
            else
            {
                Error("Ocorreu um erro ao enviar sua mensagem.");
            }

            ViewBag.Categories = GetCategories();

            return View("Index");
        }

        public List<SelectListItem> GetCategories()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            categories.Add(new SelectListItem
            {
                Text = "Dúvidas",
                Value = "Dúvidas",
                Selected = false
            });

            categories.Add(new SelectListItem
            {
                Text = "Relatar problema",
                Value = "Relatar problema",
                Selected = false
            });

            categories.Add(new SelectListItem
            {
                Text = "Sugestões e melhorias",
                Value = "Sugestões e melhorias",
                Selected = false
            });

            return categories;
        }

    }
}