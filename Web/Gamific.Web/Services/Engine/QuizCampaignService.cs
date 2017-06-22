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
    public class QuizCampaignService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile QuizCampaignService instance;


        public static QuizCampaignService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new QuizCampaignService();
                    }
                }
                return instance;
            }
        }



        #endregion

        #region Services

        public void Create(QuizCampaignEntity entity)
        {
            QuizCampaignRepository repository = new QuizCampaignRepository();
            repository.save(entity);

        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void Update(QuizCampaignEntity entity)
        {
            QuizCampaignRepository repository = new QuizCampaignRepository();
            repository.update(entity);
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public QuizCampaignEntity GetById(int id)
        {
            QuizCampaignRepository repository = new QuizCampaignRepository();
            return repository.GetById(id);
        }


        ///<summary>
        ///Atualiza questionário
        /// </summary>
        public void deleteByAssociated(int idAssociated)
        {
            QuizCampaignRepository repository = new QuizCampaignRepository();
            repository.delete(x => x.IdQuiz == idAssociated);

        }


        public void delete(int id)
        {
            QuizCampaignRepository repository = new QuizCampaignRepository();
            repository.delete(x => x.Id == id);

        }


        public List<QuizCampaignEntity> getByAssociated(int idAssociated)
        {
            QuizCampaignRepository repository = new QuizCampaignRepository();
            return repository.Get(x => x.IdQuiz == idAssociated).ToList();

        }


        #endregion
    }
}