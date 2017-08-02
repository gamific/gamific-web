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
    public class QuestionAnsweredService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile QuestionAnsweredService instance;

        private QuestionAnsweredService() : base(ENGINE_API + "questionAnswered/") { }

        public static QuestionAnsweredService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuestionAnsweredService();
                    }
                }
                return instance;
            }
        }



        #endregion

        #region Services

        public QuestionAnsweredEntity AnswerQuestion(QuestionAnsweredDTO to)
        {
            var repository = new QuestionAnsweredRepository();
            var item = new QuestionAnsweredEntity();
            //item.IdAnswers = to.IdAnswers;
            item.IdQuestion = to.IdQuestion;
            item.IdQuiz = to.IdQuiz;
            item.UserId = to.UserId;
            item.LastUpdate = DateTime.Now;
            if (QuizQuestionService.Instance.IsLastQuestion(to.IdQuiz, to.IdQuestion))
            {
                item.QuizProcess = "FINALIZADO";
            }
            else
            {
                item.QuizProcess = "RESPONDENDO";
            }

            repository.save(item);

            return item;
            
        }

        #endregion
    }
}