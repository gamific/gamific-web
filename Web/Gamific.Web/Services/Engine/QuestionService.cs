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
    public class QuestionService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile QuestionService instance;

        private QuestionService() : base(ENGINE_API + "question/") { }

        public static QuestionService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuestionService();
                    }
                }
                return instance;
            }
        }



        #endregion

        #region Services

        public void Create(QuestionEntity entity)
        {
            QuestionRepository repository = new QuestionRepository();
            repository.save(entity);
            
        }

        ///<summary>
        ///Busca a quantidade de questionários de uma firma
        /// </summary>
        public int GetCountFromFirm(int firmId, string search)
        {
            QuestionRepository repository = new QuestionRepository();
            return repository.GetCountFromFirm(firmId, search);
        }

        ///<summary>
        ///Busca os questionários de questionários de uma firma
        /// </summary>
        public List<QuestionEntity> GetAllFromFirm(int firmId, string search, int pageIndex, int pageSize)
        {

            QuestionRepository repository = new QuestionRepository();
            return repository.GetAllFromFirm(firmId, search, pageIndex, pageSize);
        }

        ///<summary>
        ///Ativa questionário
        /// </summary>
        public void Activating(int id)
        {

            QuestionRepository repository = new QuestionRepository();
            QuestionEntity entity = repository.GetById(id);
            entity.status = true;
            repository.update(entity);
        }


        ///<summary>
        ///Desativa questionário
        /// </summary>
        public void Desactivateing(int id)
        {

            QuestionRepository repository = new QuestionRepository();
            QuestionEntity entity = repository.GetById(id);
            entity.status = false;
            repository.update(entity);
        }



        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void Update(QuestionEntity quiz)
        {
            QuestionRepository repository = new QuestionRepository();
            repository.update(quiz);
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public QuestionEntity GetById(int id)
        {
            QuestionRepository repository = new QuestionRepository();
            return repository.GetById(id);
        }



        #endregion
    }
}