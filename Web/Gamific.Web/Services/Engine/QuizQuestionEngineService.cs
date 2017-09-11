using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class QuizQuestionEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile QuizQuestionEngineService instance;

        private QuizQuestionEngineService() : base(ENGINE_API + "question/") { }

        public static QuizQuestionEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuizQuestionEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public QuizQuestionEngineDTO CreateOrUpdate(QuizQuestionEngineDTO question)
        {
            return PostDTO<QuizQuestionEngineDTO>(ref question);
        }

        public QuizQuestionEngineDTO GetById(string questionId)
        {
            return GetDTO<QuizQuestionEngineDTO>(questionId);
        }

        public void DeleteById(string questionId)
        {
            Delete(questionId);
        }

        public GetAllDTO GetByQuizId(string quizId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByQuizSheetId?quizSheetId=" + quizId);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }


}