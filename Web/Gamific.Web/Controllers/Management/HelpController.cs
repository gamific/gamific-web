using System.Collections.Generic;
using System.Web.Mvc;
using Vlast.Gamific.Model.Public.Domain;
using Vlast.Gamific.Model.Public.Repository;
using Vlast.Gamific.Model.School.DTO;
using System;
using System.Linq;
using System.Transactions;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;
using HtmlAgilityPack;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/ajuda")]
    public class HelpController : BaseController
    {
        [Route("public")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR,SUPERVISOR DE CAMPANHA,SUPERVISOR DE EQUIPE,JOGADOR")]
        public ActionResult Public()
        {

            List<HelpEntity> helps = HelpRepository.Instance.GetAllFromFirm(CurrentFirm.Id);

            foreach (HelpEntity item in helps) {
                item.HelpContent = GetTextFormatted(item.HelpContent);
            }

            ViewBag.Helps = helps;
            ViewBag.TopicHelps = TopicHelpRepository.Instance.GetAllFromFirm(CurrentFirm.Id);

            return View();
        }

        /// <summary>
        /// Retorna o texto sem formatação
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetTextFormatted(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    var pageDoc = new HtmlDocument();
                    pageDoc.LoadHtml(text);

                    string formattedText = pageDoc.DocumentNode.InnerText;
                    return System.Net.WebUtility.HtmlDecode(formattedText);
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }

        [Route("{topicHelpId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
        public ActionResult Index(int topicHelpId)
        {
            ViewBag.TopicHelpId = topicHelpId;

            return View();
        }

        [Route("editar/{helpId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
        public ActionResult Edit(int helpId)
        {
            HelpEntity help = HelpRepository.Instance.GetById(helpId);

            return PartialView("_Edit", help);
        }

        [Route("cadastrar/{topicHelpId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
        public ActionResult Create(int topicHelpId)
        {
            HelpEntity help = new HelpEntity();

            help.TopicId = topicHelpId;

            return PartialView("_Edit", help);
        }

        [Route("remover/{helpId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
        public ActionResult Remove(int topicHelpId)
        {
            HelpEntity help = HelpRepository.Instance.GetById(topicHelpId);

            help.Status = GenericStatus.INACTIVE;

            HelpRepository.Instance.UpdateHelp(help);

            return Redirect("admin/ajuda/" + topicHelpId);
        }

        /// <summary>
        /// Salva as informações da ajuda sendo criada
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        [ValidateInput(false)]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
        public ActionResult Save(HelpEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    if (ModelState.IsValid)
                    {

                        if (entity.Id > 0)
                        {
                            entity.Status = GenericStatus.ACTIVE;
                            entity.FirmId = CurrentFirm.Id;
                            entity.UpdatedBy = CurrentUserId;

                            ValidateModel(entity);

                            HelpRepository.Instance.UpdateHelp(entity);

                            Success("Ajuda atualizada com sucesso.");
                            scope.Complete();
                        }
                        else
                        {
                            entity.Status = GenericStatus.ACTIVE;
                            entity.FirmId = CurrentFirm.Id;
                            entity.UpdatedBy = CurrentUserId;

                            ValidateModel(entity);

                            HelpRepository.Instance.CreateHelp(entity);

                            Success("Ajuda criada com sucesso.");
                            scope.Complete();
                        }
                    }
                    else
                    {
                        Error("Alguns campos são obrigatórios para salvar a ajuda.");

                        ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar a ajuda.");

                        return PartialView("_Edit", entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar a ajuda.");

                return PartialView("_Edit", entity);
            }

            ViewBag.TopicHelpId = entity.TopicId;

            return new EmptyResult();
        }

        [Route("search/{topicHelpId:int}")]
        [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, int topicHelpId)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<HelpEntity> searchResult = null;

                searchResult = HelpRepository.Instance.GetAllFromTopic(topicHelpId);

                var searchedQueryList = new List<HelpEntity>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.HelpTitle.ToLowerInvariant().Trim().Contains(filter))
                                        select n;

                    searchedQueryList = searchedQuery.ToList();
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
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.HelpTitle, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.HelpTitle, r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}