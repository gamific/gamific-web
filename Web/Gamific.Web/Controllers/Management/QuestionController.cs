using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/question")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class QuestionController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("editar/{id}")]
        public ActionResult Edit(int id)
        {
            QuestionService service = QuestionService.Instance;

            return PartialView("_Edit", service.GetById(id));
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            QuestionEntity entity = new QuestionEntity();

            return PartialView("_Edit", entity);
        }

        [Route("remover/{id}")]
        public ActionResult Remove(int id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    QuestionService service = QuestionService.Instance;
                    service.Desactivateing(id);
                    scope.Complete();

                    return Json(new { status = "sucess", message = "Registro removido com sucesso!" });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { status = "error", message = "Ocorreu um problema!" });
            }


        }

        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
        {

            int index = jqueryTableRequest.Page;



            if (jqueryTableRequest != null)
            {
                var all =  QuestionService.Instance.GetAllFromFirm(CurrentFirm.Id, jqueryTableRequest.Search, index, jqueryTableRequest.Length);

                var total = QuestionService.Instance.GetCountFromFirm(CurrentFirm.Id, jqueryTableRequest.Search);

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = total,
                        RecordsFiltered = all.Count(),
                        Data = all.Select(r => new string[] { r.Id.ToString(), r.Question,  r.status.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = total,
                        RecordsFiltered = all.Count(),
                        Data = all.Select(r => new string[] { r.Id.ToString(), r.Question, r.status.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("associate")]
        public ActionResult associate(List<ParserDTO> dto)
        {
            try
            {
                if (dto.Count > 0)
                {


                    using (TransactionScope delete = new TransactionScope(TransactionScopeOption.Required))
                    {
                        QuestionAnswerService.Instance.deleteByAssociated(dto[0].IdPrincipal);
                        delete.Complete();
                    }
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        for (int item = 0; item < dto.Count; item++)
                        {
                            QuestionAnswersEntity association = new QuestionAnswersEntity();
                            association.IdAnswer = dto[item].Id;
                            association.IdQuestion = dto[item].IdPrincipal;
                            association.Ordination = item;
                            association.IsRight = dto[item].IsRight;
                            QuestionAnswerService.Instance.Create(association);
                        }

                        scope.Complete();
                    }
                    return Json(new { status = "sucess", message = "Registro removido com sucesso!" });
                }
                else
                {
                    return Json(new { status = "sucess", message = "Não há registros a serem associados!" });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { status = "error", message = "Ocorreu um problema!" });
            }


        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(QuestionEntity entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (entity.Question == null)
                    {
                        return Json(new { status = "warn", message = "O campo pergunta é obrigatório!" });
                    }
                    if (entity.Id == 0)
                    {
                        QuestionService service = QuestionService.Instance;
                        entity.InitialDate = DateTime.Now;
                        entity.CreatedBy = CurrentUserEmail;
                        entity.LastUpdate = DateTime.Now;
                        entity.FirmId = CurrentFirm.Id;
                        entity.status = true;
                        service.Create(entity);
                        scope.Complete();

                        return Json(new { status = "sucess", message = "Registro cadastrado com sucesso!" });
                    }
                    else
                    {
                        QuestionService service = QuestionService.Instance;
                        entity.LastUpdate = DateTime.Now;
                        entity.status = true;
                        entity.UpdatedBy = CurrentUserEmail;
                        entity.FirmId = CurrentFirm.Id;
                        service.Update(entity);
                        scope.Complete();
                        return Json(new { status = "sucess", message = "Registro atualizado com sucesso!" });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return Json(new { status = "error", message = "Ocorreu um problema!" });
            }

        }
    }
}
