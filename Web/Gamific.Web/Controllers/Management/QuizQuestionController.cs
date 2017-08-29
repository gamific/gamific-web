using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Controllers.Management
{
    [CustomAuthorize]
    [RoutePrefix("admin/quizQuestion")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class QuizQuestionController : BaseController
    {
        static int rowsCount;

        // GET: QUESTION
        [Route("")]
        public ActionResult Index()
        {
            string quizId = Request["quizId"];

            ViewBag.QuizId = quizId;

            ViewBag.NumberOfQuestions = QuizQuestionEngineService.Instance.GetByQuizId(quizId).Count;

            return View();
        }

        [Route("editar/{questionId}")]
        public ActionResult Edit(string questionId)
        {
            QuizQuestionEngineDTO question = QuizQuestionEngineService.Instance.GetById(questionId);

            return PartialView("_Edit", question);
        }


        [Route("cadastrar/{quizId}")]
        public ActionResult Create(string quizId)
        {
            QuizQuestionEngineDTO question = new QuizQuestionEngineDTO();

            question.QuizSheetId = quizId;

            return PartialView("_Edit", question);
        }

        [Route("remover/{questionId}")]
        public ActionResult Remove(string questionId)
        {
            QuizQuestionEngineDTO question = QuizQuestionEngineService.Instance.GetById(questionId);

            try
            {
                QuizQuestionEngineService.Instance.DeleteById(questionId);
            }
            catch (Exception e)
            {
                Error("Ocorreu um erro ao remover.");
            }

            ViewBag.NumberOfQuestions = QuizQuestionEngineService.Instance.GetByQuizId(question.QuizSheetId).Count;

            return View("Index");
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(QuizQuestionEngineDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidateModel(entity);

                    string[] respostas = entity.OptionsString.Split(';');

                    int i = 0;

                    for (i = 0; i < respostas.Length; i++) {
                        entity.Options.Add(respostas[i]);
                    }

                    QuizQuestionEngineService.Instance.CreateOrUpdate(entity);

                    Success("Pergunta salva com sucesso.");
                }
                else
                {
                    ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar a pergunta.");

                    return PartialView("_Edit", entity);
                }
            }
            catch (Exception ex)
            {
                Error("Houve um erro ao salvar a pergunta.");

                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar a pergunta.");

                return PartialView("_Edit", entity);
            }

            return new EmptyResult();
        }

        [Route("search/{numberOfQuestion:int}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, int numberOfQuestion)
        {
            string quizId = Request["quizId"];

            numberOfQuestion = QuizQuestionEngineService.Instance.GetByQuizId(quizId).Count;

            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];

                List<QuizQuestionEngineDTO> searchResult = null;

                searchResult = QuizQuestionEngineService.Instance.GetByQuizId(quizId);

                var searchedQueryList = new List<QuizQuestionEngineDTO>();

                searchedQueryList = searchResult;

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.ToLowerInvariant().Trim();
                    var searchedQuery = from n in searchResult
                                        where (n.Enunciation != null && n.Enunciation.ToLowerInvariant().Trim().Contains(filter))
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
                        RecordsTotal = numberOfQuestion,
                        RecordsFiltered = numberOfQuestion,
                        Data = searchedQueryList.Select(r => new string[] { r.Enunciation, r.Subject.ToString(), r.Points.ToString(), r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = numberOfQuestion,
                        RecordsFiltered = numberOfQuestion,
                        Data = searchedQueryList.Select(r => new string[] { r.Enunciation, r.Subject.ToString(), r.Points.ToString(), r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}