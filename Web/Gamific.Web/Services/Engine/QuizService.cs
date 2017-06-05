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
            QuizRepository repository =  new QuizRepository();
            repository.save(entity);
            
        }

        ///<summary>
        ///Busca a quantidade de questionários de uma firma
        /// </summary>
        public int GetCountFromFirm(int firmId,string search)
        {
            QuizRepository repository = new QuizRepository();
            return repository.GetCountFromFirm(firmId, search);
        }

        ///<summary>
        ///Busca os questionários de questionários de uma firma
        /// </summary>
        public List<QuizEntity> GetAllFromFirm(int firmId,string search, int pageIndex,int pageSize)
        {

            QuizRepository repository = new QuizRepository();
            return repository.GetAllFromFirm(firmId, search, pageIndex, pageSize);
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
        public List<QuizDTO> GetAllFromFirmDTO(int firmId,string search, int pageIndex, int pageSize)
        {

            QuizRepository repository = new QuizRepository();

            List<QuizEntity> list = this.GetAllFromFirm(firmId, search, pageIndex, pageSize);

            List<QuizDTO> listTO = new List<QuizDTO>();

            foreach (var item in list)
            {
                QuizDTO to = new QuizDTO() {
                    CreatedBy = item.CreatedBy,
                    Description = item.Description,
                    FirmId = item.FirmId,
                    Id = item.Id,
                    IdQuizQuestion = item.IdQuizQuestion,
                    InitialDate = item.InitialDate,
                    IsMultiple = item.IsMultiple,
                    LastUpdate = item.LastUpdate,
                    Name = item.Name,
                    QuizProcess = item.QuizProcess,
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