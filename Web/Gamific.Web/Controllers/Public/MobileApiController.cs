using System.Web.Mvc;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Api.Account.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Vlast.Gamific.Web.Controllers.Account;
using System;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using System.IO;
using System.Transactions;
using Vlast.Gamific.Model.Media.Domain;
using System.Web;
using Vlast.Util.Data;
using System.Collections.Generic;
using System.Drawing;
using Vlast.Gamific.Model.Media.Repository;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;

namespace Vlast.Gamific.Web.Controllers.Mobile
{
    [Authorize]
    [RoutePrefix("mobile")]
    public class MobileApiController : BaseController
    {
        [Route("imagePath/{logoId:int}")]
        [HttpGet]
        [AllowAnonymous]
        public string GetImagePath(int logoId)
        {
            return CurrentURL + logoId;
        }

        [Route("imagePathGame")]
        [HttpPost]
        [AllowAnonymous]
        public string GetImagePathGame(string gameId, string email)
        {

            GameEngineDTO game =  GameEngineService.Instance.GetById(gameId, email);
            
            var json = JsonConvert.SerializeObject(
                   new
                   {
                       logoPath = CurrentURL + game.LogoId
                   },
                   Formatting.Indented,
                   new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                 );

            return json;
        }

        [Route("resetarSenhaMobile")]
        [HttpPost]
        [AllowAnonymous]
        public string ResetPasswordMobile(LoginViewModel model)
        {
            AuthResult result = new AuthResult();
            result.AuthStatus = AuthStatus.OK;
            result = AccountHandler.ResetPassword(new LoginRequest() { Email = model.Email, UserName = model.Email });

            string json = "";
            if (result.AuthStatus == AuthStatus.OK)
            {
                json = JsonConvert.SerializeObject(
                        new
                        {
                            message = "Nova senha gerada com sucesso ! Confirme seu e-mail para receber a nova senha."
                        },
                        Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                      );

                return json;
            }

            switch (result.AuthStatus)
            {
                default:
                    json = JsonConvert.SerializeObject(
                        new 
                        {
                            error = "Erro ao resetar a sua senha. Contate o suporte técnico."
                        },
                        Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                      );

                    return json;
            }
        }

        [Route("loginmobile")]
        [HttpPost]
        [AllowAnonymous]
        public string LoginMobile(LoginViewModel model)
        {
            AuthResult result = new AuthResult();
            PlayerEngineDTO player = null;
            string json = "";


            result = AccountHandler.Login(new LoginRequest()
            {
                UserName = model.Email,
                Password = model.Password,
                TokenMobile = model.tokenMobile,
                Device = model.tipoDispositivo
            });

            if (result.AuthStatus == AuthStatus.OK)
            {
                try
                {
                    player = PlayerEngineService.Instance.GetByEmail(model.Email);
                    player.LogoPath = GetImagePath(player.LogoId);
                }
                catch (Exception e)
                {

                }

                json = JsonConvert.SerializeObject(
                    player,
                    Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                  );

                return json;
            }

            json = JsonConvert.SerializeObject(
                    new
                    {
                        error = result.AuthStatus.ToString()
                    },
                    Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                  );

            return json;
        }


        [Route("uploadImage/{playerId}")]
        [HttpPost]
        [AllowAnonymous]
        public string UploadImage(string playerId, byte[] image)
        {
            try
            {
                WorkerDTO worker = WorkerRepository.Instance.GetDTOByExternalId(playerId);
                PlayerEngineDTO player = PlayerEngineService.Instance.GetById(playerId, worker.Email);

                ImageEntity imageSaving = new ImageEntity
                {
                    Status = GenericStatus.ACTIVE,
                    UpdatedBy = CurrentUserId
                };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    //if (player.LogoId > 0)
                    {
                       // imageSaving.Id = player.LogoId;
                    }
                    //else
                    {
                        imageSaving = ImageRepository.Instance.CreateImage(imageSaving);
                        player.LogoId = imageSaving.Id;
                        player.LogoPath = GetImagePath(player.LogoId);

                        WorkerEntity workerEntity = WorkerRepository.Instance.GetByExternalId(player.Id);
                        workerEntity.LogoId = player.LogoId;
                        WorkerRepository.Instance.UpdateWorker(workerEntity);

                        PlayerEngineService.Instance.CreateOrUpdate(player, player.Email);
                    }

                    ImageRepository.Instance.SaveOrReplaceLogo(player.LogoId, image);

                    scope.Complete();
                }

                string json = JsonConvert.SerializeObject(
                                new
                                {
                                    message = "Sucess",
                                    logoPath = GetImagePath(player.LogoId)
                                },
                                Formatting.Indented,
                                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                return json;
            }
            catch(Exception e)
            {
                string json = JsonConvert.SerializeObject(
                new
                {
                    error = "Error: " + e.Message
                },
                Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                return json;
            }

        }

        [Route("getQuizById/{id:int}")]
        [HttpGet]
        [AllowAnonymous]
        public string GetQuiz(int id)
        {
            QuizRepository repository = new QuizRepository();
            QuizCompleteDTO quiz = repository.GetQuizCompleteDTOById(id);

            string json;

            json = JsonConvert.SerializeObject(
            quiz,
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return json;
        }

        [Route("getAllQuizByGameId/{gameId}")]
        [HttpGet]
        [AllowAnonymous]
        public string GetAllQuizByGameId(string gameId)
        {
            QuizRepository repository = new QuizRepository();
            List<QuizEntity> quiz = repository.GetAllFromGame(gameId);

            string json;

            json = JsonConvert.SerializeObject(
            quiz,
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return json;
        }

        [Route("answerQuiz")]
        [HttpPost]
        [AllowAnonymous]
        public string AnswerQuiz(QuestionAnsweredEntity answer)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    QuestionAnsweredRepository questionAnswered = new QuestionAnsweredRepository();
                    questionAnswered.save(answer);

                    foreach (QuestionAnsweredSelectedEntity answered in answer.IdAnswers)
                    {
                        QuestionAnsweredSelectedRepository questionAnsweredSelected = new QuestionAnsweredSelectedRepository();
                        questionAnsweredSelected.save(new QuestionAnsweredSelectedEntity
                        {
                            AnswerId = answered.AnswerId,
                            PlayerId = answered.PlayerId,
                            QuestionId = answered.QuestionId,
                            QuizId = answered.QuizId
                        });
                    }

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                string jsonError = JsonConvert.SerializeObject(
                new
                {
                    error = "Error: " + e.Message
                },
                Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                return jsonError;
            }

            string json = JsonConvert.SerializeObject(
            new 
            {
                ok = "ok"
            },
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return json;
        }

        [Route("getQuestionsByQuizId/{idQuiz}")]
        [HttpGet]
        [AllowAnonymous]
        public string GetQuestionByQuizId(int idQuiz)
        {
            QuizQuestionRepository repository = new QuizQuestionRepository();
            List<QuizQuestionEntity> question = repository.getAllByAssociated(idQuiz);

            string json;

            json = JsonConvert.SerializeObject(
            question,
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return json;
        }
        [Route("getQuestionById/{id:int}")]
        [HttpGet]
        [AllowAnonymous]
        public string GetQuestion(int id)
        {
            QuestionRepository repository = new QuestionRepository();
            QuestionEntity question = repository.GetById(id);

            string json;

            json = JsonConvert.SerializeObject(
            question,
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return json;
        }
    }
}