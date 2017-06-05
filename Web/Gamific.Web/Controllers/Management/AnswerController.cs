using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Controllers.Public;
using Vlast.Gamific.Web.Controllers.Util;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;


namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/answer")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class AnswerController : BaseController
    {
        // GET: Metric
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("editar/{id}")]
        public ActionResult Edit(int id)
        {
            AnswerService service = AnswerService.Instance;

            return PartialView("_Edit", service.GetById(id));
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            AnswersEntity entity = new AnswersEntity();

            return PartialView("_Edit", entity);
        }

        [Route("remover/{id}")]
        public ActionResult Remove(int id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    AnswerService service = AnswerService.Instance;
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
                var all = AnswerService.Instance.GetAllFromFirm(CurrentFirm.Id, jqueryTableRequest.Search, index, jqueryTableRequest.Length);

                var total = AnswerService.Instance.GetCountFromFirm(CurrentFirm.Id, jqueryTableRequest.Search);

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = total,
                        RecordsFiltered = all.Count(),
                        Data = all.Select(r => new string[] { r.Id.ToString() , r.Name, r.Answer, r.status.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = total,
                        RecordsFiltered = all.Count(),
                        Data = all.Select(r => new string[] { r.Id.ToString(), r.Name, r.Answer, r.status.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        [Route("associate/remover/{id}")]
        public ActionResult RemoveAssociate(int id)
        {
            try
            {
                using ( TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    QuestionAnswerService.Instance.delete(id);
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


        [Route("searchAssociate/{id}")]
        public ActionResult SearchAssociate(JQueryDataTableRequest jqueryTableRequest, int id)
        {

            int index = jqueryTableRequest.Page;



            if (jqueryTableRequest != null)
            {
                var association = QuestionAnswerService.Instance.GetByQuestion(id);
                var all = new List<AnswersEntity>();
                foreach (var item in association)
                {
                    var entity = AnswerService.Instance.GetById(item.IdAnswer);
                    entity.IdAssociate = item.Id;
                    all.Add(entity);
                }
                

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsFiltered = all.Count(),
                        Data = all.Select(r => new string[] { r.Id.ToString(), r.Name, r.Answer, r.status.ToString(),r.IdAssociate.ToString() }).ToArray()

                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(AnswersEntity entity)
        {

            try
            {
                if (entity.Name == null)
                {
                    return Json(new { status = "warn", message = "O campo identificação é obrigatório!" });
                }

                if (entity.Answer == null)
                {
                    return Json(new { status = "warn", message = "O campo resposta é obrigatório!" });
                }


                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    if (entity.Id == 0)
                    {
                        AnswerService service = AnswerService.Instance;
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
                        AnswerService service = AnswerService.Instance;
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
