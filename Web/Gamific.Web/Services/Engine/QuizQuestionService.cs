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
    public class QuizQuestionService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile QuizQuestionService instance;


        public static QuizQuestionService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuizQuestionService();
                    }
                }
                return instance;
            }
        }



        #endregion

        #region Services

        public void Create(QuizQuestionEntity entity)
        {
            QuizQuestionRepository repository = new QuizQuestionRepository();
            repository.save(entity);

        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void Update(QuizQuestionEntity entity)
        {
            QuizQuestionRepository repository = new QuizQuestionRepository();
            repository.update(entity);
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public QuizQuestionEntity GetById(int id)
        {
            QuizQuestionRepository repository = new QuizQuestionRepository();
            return repository.GetById(id);
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void deleteByAssociated(int idAssociated)
        {
            QuizQuestionRepository repository = new QuizQuestionRepository();
            repository.delete(x => x.IdQuiz == idAssociated);

        }

        public List<QuizQuestionEntity> getByAssociated(int idAssociated)
        {
            QuizQuestionRepository repository = new QuizQuestionRepository();
            
           return repository.Get(x => x.IdQuiz == idAssociated).OrderBy(o => o.Ordination).ToList();

        }


        #endregion
    }
}