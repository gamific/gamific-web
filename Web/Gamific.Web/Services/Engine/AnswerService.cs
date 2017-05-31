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
    public class AnswerService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile AnswerService instance;


        public static AnswerService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new AnswerService();
                    }
                }
                return instance;
            }
        }



        #endregion

        #region Services

        public void Create(AnswersEntity entity)
        {
            AnswerRepository repository =  new AnswerRepository();
            repository.save(entity);
            
        }

        public int GetCountFromFirm(int firmId,string search)
        {
            AnswerRepository repository = new AnswerRepository();
            return repository.GetCountFromFirm(firmId, search);
        }

        public List<AnswersEntity> GetAllFromFirm(int firmId,string search, int pageIndex,int pageSize)
        {

            AnswerRepository repository = new AnswerRepository();
            return repository.GetAllFromFirm(firmId, search, pageIndex, pageSize);
        }

        public void Activating(int id)
        {

            AnswerRepository repository = new AnswerRepository();
            AnswersEntity entity = repository.GetById(id);
            entity.status = true;
            repository.update(entity);
        }

        public void Desactivateing(int id)
        {

            AnswerRepository repository = new AnswerRepository();
            AnswersEntity entity = repository.GetById(id);
            entity.status = false;
            repository.update(entity);
        }

        public void Update(AnswersEntity entity)
        {
            AnswerRepository repository = new AnswerRepository();
            repository.update(entity);
        }

        public AnswersEntity GetById(int id)
        {
            AnswerRepository repository = new AnswerRepository();
            return repository.GetById(id);
        }



        #endregion
    }
}