using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/questionAnswer")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class QuestionAnswerController : BaseController
    {
        static int rowsCount;

        // GET: ANSWER
        [Route("")]
        public ActionResult Index()
        {
            string questionId = Request["questionId"];

            ViewBag.QuestionId = questionId;

            ViewBag.NumberOfAnswers = 0;// WorkerRepository.Instance.GetCountFromFirm(CurrentFirm.Id);

            return View();
        }

        [Route("editar/{answerId}")]
        public ActionResult Edit(string answerId)
        {
            QuestionAnswerEngineDTO answer = new QuestionAnswerEngineDTO();//WorkerRepository.Instance.GetDTOById(workerId);

            return PartialView("_Edit", answer);
        }


        [Route("cadastrar/{questionId}")]
        public ActionResult Create(string questionId)
        {
            QuestionAnswerEngineDTO answer = new QuestionAnswerEngineDTO();

            answer.QuestionId = questionId;

            return PartialView("_Edit", answer);
        }

        [Route("remover/{answerId:int}")]
        public ActionResult Remove(int answerId)
        {
            try
            {
                QuestionAnswerEngineDTO answer = new QuestionAnswerEngineDTO();//WorkerRepository.Instance.GetDTOById(workerId);

                //WorkerRepository.Instance.UpdateWorker(quiz);
            }
            catch (Exception e)
            {
                Error("Ocorreu um erro ao remover.");
            }

            ViewBag.NumberOfAnswers = 0;//.Instance.GetCountFromFirm(CurrentFirm.Id);

            return View("Index");
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(QuestionAnswerEngineDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidateModel(entity);

                    //PlayerEngineService.Instance.CreateOrUpdate(player);

                    Success("Resposta salva com sucesso.");
                }
                else
                {
                    ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar a resposta.");

                    return PartialView("_Edit", entity);
                }
            }
            catch (Exception ex)
            {
                Error("Houve um erro ao salvar a resposta.");

                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar a resposta.");

                return PartialView("_Edit", entity);
            }

            return new EmptyResult();
        }

        [Route("search/{numberOfAnswers:int}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, int numberOfAnswers)
        {
            numberOfAnswers = 0;//WorkerRepository.Instance.GetCountFromFirm(CurrentFirm.Id);

            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];

                List<QuestionAnswerEngineDTO> searchResult = null;

                searchResult = new List<QuestionAnswerEngineDTO>();//WorkerRepository.Instance.GetAllFromFirm(CurrentFirm.Id, jqueryTableRequest.Page, 10);

                var searchedQueryList = new List<QuestionAnswerEngineDTO>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.Title != null && n.Title.ToLowerInvariant().Trim().Contains(filter))
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
                        RecordsTotal = numberOfAnswers,
                        RecordsFiltered = numberOfAnswers,
                        Data = searchedQueryList.Select(r => new string[] { r.Title, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = numberOfAnswers,
                        RecordsFiltered = numberOfAnswers,
                        Data = searchedQueryList.Select(r => new string[] { r.Title, r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}