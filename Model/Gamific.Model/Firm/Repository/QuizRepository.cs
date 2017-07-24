using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Properties;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{

    /// <summary>
    /// Consulta de dados relacionados com o funcionario
    /// </summary>
    public class QuizRepository : GenericRepository<QuizEntity>
    {
        #region Singleton instance
        /*
        protected static object _syncRoot = new Object();
        private static volatile QuizRepository instance;

        private QuizRepository() { }

        public static QuizRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuizRepository();
                    }
                }

                return instance;
            }
        }
        */
        #endregion

        ///<summary>
        ///Busca todos os funcionarios do perfil jogador de uma firma
        /// </summary>
        public List<QuizEntity> GetAllFromFirm(int firmId, string search,int pageIndex,int pageSize)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from q in context.QuizEntity where (q.FirmId == firmId && q.Name.Contains(search)) select q).OrderBy(x => x.Name).Skip(pageIndex * pageSize).Take(pageSize);

                return query.ToList();
            }
        }

        public List<QuizEntity> GetAllFromFirmForApp(int firmId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = (from q in context.QuizEntity where (q.FirmId == firmId && q.DateLimit >= DateTime.Now && q.status == true) select q);

                return query.ToList();
            }
        }

        public List<QuizEntity> GetAllFromGame(string gameId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from quiz in context.QuizEntity
                            where quiz.GameId == gameId
                            && quiz.DateLimit >= DateTime.Now
                            select quiz;

                return query.ToList();
            }
        }

        public List<QuizEntity> GetAllFromEpisode(string episodeId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from quiz in context.QuizEntity
                            from quizEpisode in context.QuizCampaignEntity
                            where quizEpisode.IdCampaign == episodeId
                            && quizEpisode.IdQuiz == quiz.Id
                            && quiz.DateLimit <= DateTime.Now
                            select quiz;

                return query.ToList();
            }
        }

        public QuizCompleteDTO GetQuizCompleteDTOById(int quizId)
        {
            using (ModelContext context = new ModelContext())
            {
                var joinQuery = (from a in context.AnswersEntity
                            join questionAnswer in context.QuestionAnswersEntity on a.Id equals questionAnswer.IdAnswer
                            select new AnswersDTO
                            {
                                Id = a.Id,
                                Answer = a.Answer,
                                IdQuestion = questionAnswer.IdQuestion,
                                IsCorrect = questionAnswer.IsRight
                            }).ToList();

                var answers = (from b in joinQuery
                              group b by b.IdQuestion into g
                              select g.ToList()).ToList();

                var questions = (from quizQuestion in context.QuizQuestionEntity
                                 from question in context.QuestionEntity
                                 where quizQuestion.IdQuiz == quizId
                                 && quizQuestion.IdQuestion == question.Id
                                 select new QuestionDTO
                                 {
                                     Question = question.Question,
                                     UpdatedBy = question.UpdatedBy,
                                     CreatedBy = question.CreatedBy,
                                     InitialDate = question.InitialDate,
                                     Link = question.Link,
                                     LastUpdate = question.LastUpdate,
                                     Id = question.Id,
                                     Required = question.Required,
                                     status = question.status
                                  }).ToList();

                foreach(QuestionDTO s in questions)
                {
                    s.answers = (from answer in answers
                                 where answer.FirstOrDefault().IdQuestion == s.Id
                                 select answer).FirstOrDefault();
                }

                QuizCompleteDTO query = (from quiz in context.QuizEntity
                             from quizQuestion in context.QuizQuestionEntity
                             from question in context.QuestionEntity
                             where quizQuestion.IdQuiz == quizId
                             && quizQuestion.IdQuestion == question.Id
                             select new QuizCompleteDTO
                             {
                                 DateLimit = quiz.DateLimit,
                                 GameId = quiz.GameId,
                                 InitialDate = quiz.InitialDate,
                                 Id = quiz.Id,
                                 Name = quiz.Name,
                                 IdQuizQuestion = quiz.IdQuizQuestion,
                                 Description = quiz.Description,
                                 IsMultiple = quiz.IsMultiple,
                                 Required = quiz.Required,
                                 Score = quiz.Score,
                                 CreatedBy = quiz.CreatedBy,
                                 LastUpdate = quiz.LastUpdate,
                                 Link = quiz.Link,
                                 status = quiz.status,
                                 UpdatedBy = quiz.UpdatedBy
                             }).FirstOrDefault();

                if(query != null)
                {
                    query.questions = questions;
                }

                return query;
            }   
        }

        ///<summary>
        ///Busca todos os funcionarios do perfil jogador de uma firma
        /// </summary>
        public int GetCountFromFirm(int firmId, string search)
        {
            using (ModelContext context = new ModelContext())
            {
                if (search != null)
                {
                    return (from q in context.QuizEntity where (q.FirmId == firmId && q.Name.Contains(search)) select q).Count();
                }
                return (from q in context.QuizEntity where (q.FirmId == firmId) select q).Count();
            }
        }
    }
}
