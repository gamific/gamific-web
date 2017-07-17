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
    [RoutePrefix("admin/quiz")]
    [CustomAuthorize(Roles = "WORKER,ADMINISTRADOR")]
    public class QuizController : BaseController
    {

        List<ParserDTO> arrayQuiz = new List<ParserDTO>();


        // GET: Metric
        [Route("")]
        public ActionResult Index()
        {
            ParserDTO to = new ParserDTO();
            to.Description = "Não há perguntas selecionadas";
            arrayQuiz.Add(to);


            ViewBag.arrayQuiz = arrayQuiz;
            return View();
        }


        [Route("paginaQuiz") ]
        public ActionResult paginaQuiz()
        {
            return PartialView("_QuizAssociate");
        }

        [Route("paginaEpisode") ]
        public ActionResult paginaEpisode()
        {
            return PartialView("_EpisodeAssociate");
        }

        [Route("paginaQuizComplete")]
        public ActionResult paginaQuizComplete()
        {
            return PartialView("_QuizComplete");
        }

        [Route("paginaQuestionComplete")]
        public ActionResult paginaQuestionComplete()
        {
            return PartialView("_QuestionComplete");
        }

        [Route("paginaEpisodeComplete")]
        public ActionResult paginaEpisodeComplete()
        {
            return PartialView("_EpisodeComplete");
        }

        [Route("paginaQuestion")]
        public ActionResult paginaQuestion()
        {
            return PartialView("_QuestionAssociate");
        }

        [Route("add")]
        [HttpPost]
        public void add(ParserDTO json)
        {
            arrayQuiz.Add(json);
            ViewBag.arrayQuiz = arrayQuiz;
        }

        [Route("editar/{id}")]
        public ActionResult Edit(int id)
        {
            QuizService service = QuizService.Instance;

            return PartialView("_Edit", service.GetById(id));
        }

        [Route("cadastrar")]
        public ActionResult Create()
        {
            QuizEntity entity = new QuizEntity();

            return PartialView("_Edit", entity);
        }

        [Route("remover/{id}")]
        public ActionResult Remove(int id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    QuizService service = QuizService.Instance;
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


        [Route("associate")]
        public ActionResult associate(List<ParserDTO> dto)
        {
            try
            {
                if (dto.Count > 0)
                {


                    using (TransactionScope delete = new TransactionScope(TransactionScopeOption.Required))
                    {
                        QuizQuestionService.Instance.deleteByAssociated(dto[0].IdPrincipal);
                        delete.Complete();
                    }
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        for (int item = 0; item < dto.Count; item++)
                        {
                            QuizQuestionEntity association = new QuizQuestionEntity();
                            association.IdQuestion = dto[item].Id;
                            association.IdQuiz = dto[item].IdPrincipal;
                            association.Ordination = item;
                            QuizQuestionService.Instance.Create(association);
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


        [Route("episode")]
        public ActionResult associateEpisode(List<ParserDTO> dto)
        {
            try
            {
                if (dto.Count > 0)
                {

                    using (TransactionScope delete = new TransactionScope(TransactionScopeOption.Required))
                    {
                        QuizCampaignService.Instance.deleteByAssociated(dto[0].IdPrincipal);
                        delete.Complete();
                    }
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        for (int item = 0; item < dto.Count; item++)
                        {
                            QuizCampaignEntity association = new QuizCampaignEntity();
                            association.IdCampaign = dto[item].IdString;
                            association.IdQuiz = dto[item].IdPrincipal;
                            QuizCampaignService.Instance.Create(association);
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

        [Route("complete/{id}")]
        public ActionResult GetComplete(int id)
        {

            var toReturn = new List<Services.Engine.DTO.QuizCompleteDTO>();

            var quiz = QuizService.Instance.GetById(id);
            var questionAssociations = QuizQuestionService.Instance.getByAssociated(id);

            foreach (var item in questionAssociations)
            {
                var to = new Services.Engine.DTO.QuizCompleteDTO();

                to.QuestionEntity = QuestionService.Instance.GetById(item.IdQuestion);
                to.QuizEntity = quiz;
                var answerAssociations = QuestionAnswerService.Instance.GetByQuestion(item.IdQuestion);
                to.AnswersEntity = new List<AnswersEntity>();
                foreach (var answer in answerAssociations)
                {
                    to.AnswersEntity.Add(AnswerService.Instance.GetById(answer.IdAnswer));
                }

                toReturn.Add(to);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);

        }

        [Route("search")]
        public ActionResult Search(JQueryDataTableRequest jqueryTableRequest)
        {

            int index = jqueryTableRequest.Page;



            if (jqueryTableRequest != null)
            {
                var all = QuizService.Instance.GetAllFromFirmDTO(CurrentFirm.Id, jqueryTableRequest.Search, index, jqueryTableRequest.Length);

                var total = QuizService.Instance.GetCountFromFirm(CurrentFirm.Id, jqueryTableRequest.Search);

                JQueryDataTableResponse response = null;

                if (jqueryTableRequest.Type == null || jqueryTableRequest.Type.Equals("asc"))
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = total,
                        RecordsFiltered = all.Count(),
                        Data = all.Select(r => new string[] { r.Id.ToString(), r.Name, r.QtdPerguntas.ToString(),r.status.ToString() }).ToArray().OrderBy(item => item[index]).ToArray()

                    };
                }
                else
                {
                    response = new JQueryDataTableResponse()
                    {
                        Draw = jqueryTableRequest.Draw,
                        RecordsTotal = total,
                        RecordsFiltered = all.Count(),
                        Data = all.Select(r => new string[] { r.Id.ToString(), r.Name, r.QtdPerguntas.ToString(), r.status.ToString() }).ToArray().OrderByDescending(item => item[index]).ToArray()
                    };
                }

                return new DataContractResult() { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("salvar")]
        [HttpPost]
        public ActionResult Save(QuizEntity entity)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (ModelState.IsValid)
                    {
                        if (entity.Name == null)
                        {
                            //return Json(new { status = "warn", message = "O campo nome é obrigatório!" });
                            ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar o Questionário.");
                            return PartialView("_Edit", entity);

                        }
                        if (entity.Id == 0)
                        {
                            QuizService service = QuizService.Instance;
                            entity.InitialDate = DateTime.Now;
                            entity.CreatedBy = CurrentUserEmail;
                            entity.LastUpdate = DateTime.Now;
                            entity.FirmId = CurrentFirm.Id;
                            entity.status = true;
                            service.Create(entity);
                            Success("Questionário cadastrado com sucesso.");
                            scope.Complete();

                           // return Json(new { status = "sucess", message = "Registro cadastrado com sucesso!" });
                        }
                        else
                        {
                            QuizService service = QuizService.Instance;
                            entity.LastUpdate = DateTime.Now;
                            entity.status = true;
                            entity.UpdatedBy = CurrentUserEmail;
                            entity.FirmId = CurrentFirm.Id;
                            service.Update(entity);
                            Success("Questionário atualizado com sucesso.");
                            scope.Complete();
                           // return Json(new { status = "sucess", message = "Registro atualizado com sucesso!" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Alguns campos são obrigatórios para salvar o questionário.");
                        return PartialView("_Edit", entity);

                    }
                } 

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                //return Json(new { status = "error", message = "Ocorreu um problema!" });
                ModelState.AddModelError("", "Ocorreu um problema!");
                return PartialView("_Edit", entity);
            }
            return new EmptyResult();

        }


    }
}
