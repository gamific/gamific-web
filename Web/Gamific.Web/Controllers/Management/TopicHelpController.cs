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

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/topicoAjuda")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class TopicHelpController : BaseController
    {
        // GET: Topic Help
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("editar/{topicHelpId:int}")]
        public ActionResult Edit(int topicHelpId)
        {
            TopicHelpEntity topicHelp = TopicHelpRepository.Instance.GetById(topicHelpId);

            return PartialView("_Edit", topicHelp);
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            TopicHelpEntity topicHelp = new TopicHelpEntity();

            return PartialView("_Edit", topicHelp);
        }

        [Route("remover/{topicHelpId:int}")]
        public ActionResult Remove(int topicHelpId)
        {

            List<HelpEntity> helps = HelpRepository.Instance.GetAllFromTopic(topicHelpId);

            foreach (HelpEntity item in helps) {
                item.Status = GenericStatus.INACTIVE;

                HelpRepository.Instance.UpdateHelp(item);
            }

            TopicHelpEntity topicHelp = TopicHelpRepository.Instance.GetById(topicHelpId);

            topicHelp.Status = GenericStatus.INACTIVE;

            TopicHelpRepository.Instance.UpdateTopicHelp(topicHelp);

            return View("Index");
        }

        /// <summary>
        /// Salva as informações do topico de ajuda sendo criado
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(TopicHelpEntity entity)
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

                            TopicHelpRepository.Instance.UpdateTopicHelp(entity);

                            Success("Topico de ajuda atualizado com sucesso.");
                            scope.Complete();
                        }
                        else
                        {
                            entity.Status = GenericStatus.ACTIVE;
                            entity.FirmId = CurrentFirm.Id;
                            entity.UpdatedBy = CurrentUserId;

                            ValidateModel(entity);

                            TopicHelpRepository.Instance.CreateTopicHelp(entity);

                            Success("Topico de ajuda criado com sucesso.");
                            scope.Complete();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar o topico de ajuda.");

                        return PartialView("_Edit", entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar o topico de ajuda.");

                return PartialView("_Edit", entity);
            }

            return new EmptyResult();
        }

        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<TopicHelpEntity> searchResult = null;

                searchResult = TopicHelpRepository.Instance.GetAllFromFirm(CurrentFirm.Id);

                var searchedQueryList = new List<TopicHelpEntity>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.TopicName.ToLowerInvariant().Trim().Contains(filter))
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
                        Data = searchedQueryList.Select(r => new string[] { r.TopicName, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = (jqueryTableRequest.Page + 1) * 10 - (10 - searchedQueryList.Count),
                        RecordsFiltered = (jqueryTableRequest.Page + 1) * 10 + 1,
                        Data = searchedQueryList.Select(r => new string[] { r.TopicName, r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}