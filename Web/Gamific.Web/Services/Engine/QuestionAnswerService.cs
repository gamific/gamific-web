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
    public class QuestionAnswerService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile QuestionAnswerService instance;


        public static QuestionAnswerService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuestionAnswerService();
                    }
                }
                return instance;
            }
        }



        #endregion

        #region Services

        public void Create(QuestionAnswersEntity entity)
        {
            QuestionAnswerRepository repository = new QuestionAnswerRepository();
            repository.save(entity);

        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void Update(QuestionAnswersEntity entity)
        {
            QuestionAnswerRepository repository = new QuestionAnswerRepository();
            repository.update(entity);
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public QuestionAnswersEntity GetById(int id)
        {
            QuestionAnswerRepository repository = new QuestionAnswerRepository();
            return repository.GetById(id);
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void deleteByAssociated(int idAssociated)
        {
            try
            {
                QuestionAnswerRepository repository = new QuestionAnswerRepository();
                repository.delete(x => x.IdQuestion == idAssociated);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void delete(int id)
        {
            try
            {
                QuestionAnswerRepository repository = new QuestionAnswerRepository();
                repository.delete(x => x.Id == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<QuestionAnswersEntity> GetByQuestion(int questionId)
        {
            try
            {
                QuestionAnswerRepository repository = new QuestionAnswerRepository();
                return repository.Get(x => x.IdQuestion == questionId).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }
}