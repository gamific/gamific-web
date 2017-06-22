using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Instrumentation;


namespace Vlast.Gamific.Web.Services.Engine
{
    public class QuizService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile QuizService instance;

        private QuizService() : base(ENGINE_API + "metric/") { }

        public static QuizService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuizService();
                    }
                }
                return instance;
            }
        }



        #endregion

        #region Services

        public void Create(QuizEntity entity)
        {
            QuizRepository repository = new QuizRepository();
            repository.save(entity);

        }

        ///<summary>
        ///Busca a quantidade de questionários de uma firma
        /// </summary>
        public int GetCountFromFirm(int firmId, string search)
        {
            QuizRepository repository = new QuizRepository();
            return repository.GetCountFromFirm(firmId, search);
        }

        ///<summary>
        ///Busca os questionários de questionários de uma firma
        /// </summary>
        public List<QuizEntity> GetAllFromFirm(int firmId, string search, int pageIndex, int pageSize)
        {

            QuizRepository repository = new QuizRepository();
            return repository.GetAllFromFirm(firmId, search, pageIndex, pageSize);
        }



        /// <summary>
        /// Busca todos questionarios por usuario e firm
        /// </summary>
        /// <param name="firmId"></param>
        /// <returns></returns>
        public List<QuizEntity> GetAllFromFirmForApp(int firmId)
        {

            QuizRepository repository = new QuizRepository();
            return repository.GetAllFromFirmForApp(firmId);
        }



        ///<summary>
        ///Ativa questionário
        /// </summary>
        public void Activating(int id)
        {

            QuizRepository repository = new QuizRepository();
            QuizEntity quiz = repository.GetById(id);
            quiz.status = true;
            repository.update(quiz);
        }


        ///<summary>
        ///Desativa questionário
        /// </summary>
        public void Desactivateing(int id)
        {

            QuizRepository repository = new QuizRepository();
            QuizEntity quiz = repository.GetById(id);
            quiz.status = false;
            repository.update(quiz);
        }



        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void Update(QuizEntity quiz)
        {
            QuizRepository repository = new QuizRepository();
            repository.update(quiz);
        }

        public List<Model.Firm.Domain.QuizCompleteDTO> GetQuiz(int firmId, int userId)
        {
            var toReturn = new List<Model.Firm.Domain.QuizCompleteDTO>();

            var quizList = this.GetAllFromFirmForApp(firmId);
            foreach (var quiz in quizList)
            {
                var quizComplete = new Model.Firm.Domain.QuizCompleteDTO();
                var questionAssociations = QuizQuestionService.Instance.getByAssociated(quiz.Id);

                quizComplete.Id = quiz.Id;
                quizComplete.CreatedBy = quiz.CreatedBy;
                quizComplete.DateLimit = quiz.DateLimit;
                quizComplete.Description = quiz.Description;
                quizComplete.FirmId = quiz.FirmId;
                quizComplete.IdQuizQuestion = quiz.IdQuizQuestion;
                quizComplete.InitialDate = quiz.InitialDate;
                quizComplete.IsMultiple = quiz.IsMultiple;
                quizComplete.LastUpdate = quiz.LastUpdate;
                quizComplete.Link = quiz.Link;
                quizComplete.Name = quiz.Name;
                quizComplete.questions = new List<QuestionDTO>();
                quizComplete.Required = quiz.Required;
                quizComplete.Score = quiz.Score;
                quizComplete.status = quiz.status;
                quizComplete.UpdatedBy = quiz.UpdatedBy;

                foreach (var item in questionAssociations)
                {
                    var question = QuestionService.Instance.GetById(item.IdQuestion);
                    var questionDto = new QuestionDTO();
                    questionDto.CreatedBy = question.CreatedBy;
                    questionDto.FirmId = question.FirmId;
                    questionDto.Id = question.Id;
                    questionDto.IdQuestionAnswered = question.IdQuestionAnswered;
                    questionDto.IdQuestionAnswers = question.IdQuestionAnswers;
                    questionDto.InitialDate = question.InitialDate;
                    questionDto.LastUpdate = question.LastUpdate;
                    questionDto.Link = question.Link;
                    questionDto.Question = question.Question;
                    questionDto.Required = question.Required;
                    questionDto.status = question.status;
                    questionDto.UpdatedBy = question.UpdatedBy;

                    questionDto.answers = new List<AnswersEntity>();
                    quizComplete.questions.Add(questionDto);
                    var answerAssociations = QuestionAnswerService.Instance.GetByQuestion(item.IdQuestion);
                    foreach (var answer in answerAssociations)
                    {
                        questionDto.answers.Add(AnswerService.Instance.GetById(answer.IdAnswer));
                    }

                    toReturn.Add(quizComplete);
                }
            }

            return toReturn;
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public QuizEntity GetById(int id)
        {
            QuizRepository repository = new QuizRepository();
            return repository.GetById(id);
        }


        ///<summary>
        ///Busca os questionários de questionários de uma firma
        /// </summary>
        public List<QuizDTO> GetAllFromFirmDTO(int firmId, string search, int pageIndex, int pageSize)
        {

            QuizRepository repository = new QuizRepository();

            List<QuizEntity> list = this.GetAllFromFirm(firmId, search, pageIndex, pageSize);

            List<QuizDTO> listTO = new List<QuizDTO>();

            foreach (var item in list)
            {
                QuizDTO to = new QuizDTO()
                {
                    CreatedBy = item.CreatedBy,
                    Description = item.Description,
                    FirmId = item.FirmId,
                    Id = item.Id,
                    IdQuizQuestion = item.IdQuizQuestion,
                    InitialDate = item.InitialDate,
                    IsMultiple = item.IsMultiple,
                    LastUpdate = item.LastUpdate,
                    Name = item.Name,
                    Required = item.Required,
                    Score = item.Score,
                    UpdatedBy = item.UpdatedBy,
                    DateLimit = item.DateLimit,
                    status = item.status,
                };
                QuizQuestionRepository questionRepository = new QuizQuestionRepository();

                to.QtdPerguntas = questionRepository.Get(x => x.IdQuiz == item.Id).Count();
                listTO.Add(to);
            }

            return listTO;
        }

        #endregion
    }
}