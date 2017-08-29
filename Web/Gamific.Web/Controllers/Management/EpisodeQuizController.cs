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
    [RoutePrefix("admin/episodeQuiz")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class EpisodeQuizController : BaseController
    {
        static int rowsCount;

        // GET: QUIZ
        [Route("")]
        public ActionResult Index()
        {
            string episodeId = Request["episodeId"];

            ViewBag.EpisodeId = episodeId;

            ViewBag.NumberOfQuiz = EpisodeQuizEngineService.Instance.GetByEpisodeId(episodeId).Count;

            return View();
        }

        [Route("editar/{quizId}")]
        public ActionResult Edit(string quizId)
        {
            EpisodeQuizEngineDTO quiz = EpisodeQuizEngineService.Instance.GetById(quizId);

            return PartialView("_Edit", quiz);
        }

        [Route("cadastrar/{episodeId}")]
        public ActionResult Create(string episodeId)
        {
            EpisodeQuizEngineDTO quiz = new EpisodeQuizEngineDTO();

            quiz.EpisodeId = episodeId;

            return PartialView("_Edit", quiz);
        }

        [Route("remover/{quizId}")]
        public ActionResult Remove(string quizId)
        {
            EpisodeQuizEngineDTO quiz = EpisodeQuizEngineService.Instance.GetById(quizId);

            try
            {
                EpisodeQuizEngineService.Instance.DeleteById(quizId);
            }
            catch (Exception e)
            {
                Error("Ocorreu um erro ao remover.");
            }

            ViewBag.NumberOfQuiz = EpisodeQuizEngineService.Instance.GetByEpisodeId(quiz.EpisodeId).Count;

            return View("Index");
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(EpisodeQuizEngineDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidateModel(entity);

                    EpisodeQuizEngineService.Instance.CreateOrUpdate(entity);

                    Success("Quiz salvo com sucesso.");
                }
                else
                {
                    ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar o quiz.");

                    return PartialView("_Edit", entity);
                }
            }
            catch (Exception ex)
            {
                Error("Houve um erro ao salvar o quiz.");

                Logger.LogException(ex);

                ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar o quiz.");

                return PartialView("_Edit", entity);
            }

            return new EmptyResult();
        }

        [Route("search/{numberOfQuiz:int}")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest, int numberOfQuiz)
        {
            string episodeId = Request["episodeId"];

            numberOfQuiz = EpisodeQuizEngineService.Instance.GetByEpisodeId(episodeId).Count;

            if (jqueryTableRequest != null)
            {
                string filter = "";

                string[] searchTerms = jqueryTableRequest.Search.Split(new string[] { "#;$#" }, StringSplitOptions.None);
                filter = searchTerms[0];

                List<EpisodeQuizEngineDTO> searchResult = null;

                searchResult = EpisodeQuizEngineService.Instance.GetByEpisodeId(episodeId);

                var searchedQueryList = new List<EpisodeQuizEngineDTO>();

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
                        RecordsTotal = numberOfQuiz,
                        RecordsFiltered = numberOfQuiz,
                        Data = searchedQueryList.Select(r => new string[] { r.Title, r.Id.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = numberOfQuiz,
                        RecordsFiltered = numberOfQuiz,
                        Data = searchedQueryList.Select(r => new string[] { r.Title, r.Id.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("cadastrarPergunta/{quizId}")]
        public ActionResult CreateQuizQuestion(string quizId)
        {
            return Redirect("/admin/quizQuestion?quizId=" + quizId);
        }

    }
}