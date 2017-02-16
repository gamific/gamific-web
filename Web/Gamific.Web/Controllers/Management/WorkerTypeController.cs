using System.Collections.Generic;
using System.Web.Mvc;
using Vlast.Gamific.Model.School.DTO;
using System;
using System.Linq;
using System.Transactions;
using Vlast.Util.Data;
using Vlast.Util.Instrumentation;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Firm.DTO;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/funcoes")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class WorkerTypeController : BaseController
    {
        // GET: Worker Type
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.NumberOfWorkerTypes = WorkerTypeRepository.Instance.GetCountFromFirm(CurrentFirm.ExternalId);

            return View();
        }

        [Route("editar/{workerTypeId:int}")]
        public ActionResult Edit(int workerTypeId)
        {
            WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(workerTypeId);

            ViewBag.Profiles = GetProfilesToSelect(workerType.ProfileName);

            return PartialView("_Edit", workerType);
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            WorkerTypeEntity workerType = new WorkerTypeEntity();

            ViewBag.Profiles = GetProfilesToSelect(new Profiles());

            return PartialView("_Edit", workerType);
        }

        [Route("remover/{workerTypeId:int}")]
        public ActionResult Remove(int workerTypeId)
        {

            List<WorkerEntity> worker = WorkerRepository.Instance.GetAllByWorkerType(workerTypeId);

            if (worker.Count > 0)
            {
                Error("Existem funcionários vinculados a essa função, não é possível excluir nesse caso.");
            }
            else
            {
                WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(workerTypeId);

                workerType.Status = GenericStatus.INACTIVE;

                WorkerTypeRepository.Instance.UpdateWorkerType(workerType);
            }

            ViewBag.NumberOfWorkerTypes = WorkerTypeRepository.Instance.GetCountFromFirm(CurrentFirm.ExternalId);

            return View("Index");
        }

        /// <summary>
        /// Salva as informações do tipo de jogador
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(WorkerTypeEntity entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    ViewBag.Profiles = GetProfilesToSelect(entity.ProfileName);

                    if (ModelState.IsValid)
                    {

                        if (entity.Id > 0)
                        {
                            entity.Status = GenericStatus.ACTIVE;
                            entity.FirmId = CurrentFirm.Id;
                            entity.ExternalFirmId = CurrentFirm.ExternalId;
                            entity.UpdatedBy = CurrentUserId;

                            ValidateModel(entity);

                            WorkerTypeRepository.Instance.UpdateWorkerType(entity);

                            Success("Função atualizada com sucesso.");
                            scope.Complete();
                        }
                        else
                        {
                            entity.Status = GenericStatus.ACTIVE;
                            entity.FirmId = CurrentFirm.Id;
                            entity.ExternalFirmId = CurrentFirm.ExternalId;
                            entity.UpdatedBy = CurrentUserId;

                            ValidateModel(entity);

                            WorkerTypeRepository.Instance.CreateWorkerType(entity);

                            Success("Função criada com sucesso.");
                            scope.Complete();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar a Função.");

                        return PartialView("_Edit", entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar a Função.");

                return PartialView("_Edit", entity);
            }

            return new EmptyResult();
        }

        [Route("search/{numberOfWorkerTypes:int}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, int numberOfWorkerTypes)
        {
            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];
                List<WorkerTypeEntity> searchResult = null;

                searchResult = WorkerTypeRepository.Instance.GetAllFromFirm(CurrentFirm.ExternalId, jqueryTableRequest.Page, 10);

                var searchedQueryList = new List<WorkerTypeEntity>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.TypeName.ToLowerInvariant().Trim().Contains(filter))
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
                        RecordsTotal = numberOfWorkerTypes,
                        RecordsFiltered = numberOfWorkerTypes,
                        Data = searchedQueryList.Select(r => new string[] { r.TypeName, r.ProfileName.ToString(), r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()
                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = numberOfWorkerTypes,
                        RecordsFiltered = numberOfWorkerTypes,
                        Data = searchedQueryList.Select(r => new string[] { r.TypeName, r.ProfileName.ToString(), r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cria a lista de seleção dos perfis
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetProfilesToSelect(Profiles selected)
        {
            IEnumerable<Profiles> profiles = Enum.GetValues(typeof(Profiles)).Cast<Profiles>();

            var query = from c in profiles
                        select new SelectListItem
                        {
                            Text = c.ToString(),
                            Value = c.ToString(),
                            Selected = c == selected
                        };

            return query.ToList();
        }

    }
}