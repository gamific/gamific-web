using System.Collections.Generic;
using System.Web.Mvc;
using Vlast.Gamific.Model.Public.Domain;
using Vlast.Gamific.Model.Public.Repository;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Util.Data;
using System;
using System.Linq;
using System.Transactions;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Instrumentation;
using Vlast.Gamific.Model.Firm.Repository;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [RoutePrefix("admin/param")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class ParamController : BaseController
    {
        // GET: Param
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }


        [Route("cadastrar")]
        public ActionResult Create()
        {
            ParamEntity param = new ParamEntity();

            return PartialView("_Edit", param);
        }


        /// <summary>
        /// Salva as informações do topico de ajuda sendo criado
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(ParamEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    if (ModelState.IsValid)
                    {

                        if (entity.Id > 0)
                        {
                            entity.GameId = CurrentFirm.ExternalId;
                            entity.UpdateBy = CurrentUserId;

                            ValidateModel(entity);

                            ParamRepository.Instance.UpdateParam(entity);

                            Success("Topico de ajuda atualizado com sucesso.");
                            scope.Complete();
                        }
                        else
                        {
                            entity.GameId = CurrentFirm.ExternalId;
                            entity.UpdateBy = CurrentUserId;

                            ValidateModel(entity);

                            ParamRepository.Instance.CreateParam(entity);

                            Success("Parametro criado com sucesso.");
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
            int index = 0;
            if (jqueryTableRequest.Order != null)
            {
                index = Int32.Parse(jqueryTableRequest.Order);
            }

            if (jqueryTableRequest != null)
            {
                string gameId = CurrentFirm.ExternalId;
                List<ParamEntity> paramList = ParamRepository.Instance.GetAll(gameId, jqueryTableRequest.Page);
                int count = ParamRepository.Instance.GetCountFromGame(gameId);

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = count,
                        RecordsFiltered = count,
                        Data = paramList.Select(r => new string[] { r.Name, r.Value, r.Description, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = count,
                        RecordsFiltered = count,
                        Data = paramList.Select(r => new string[] { r.Name, r.Value, r.Description, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }

        [Route("editar/{paramId}")]
        public ActionResult Edit(int paramId)
        {
            ParamEntity param = ParamRepository.Instance.GetById(paramId);
            param.GameId = CurrentFirm.ExternalId;


            return PartialView("_edit", param);
        }

        [Route("remover/{paramId:int}")]
        public ActionResult Remove(int paramId)
        {

            ParamRepository.Instance.DeleteById(paramId);


            return View("Index");
        }
    }
}