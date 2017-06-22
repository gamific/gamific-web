using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Services.Account;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Api.Account
{
    /// <summary>
    /// Autenticação e criação de usuários
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/quiz")]
    public class QuizAPIController : ApiController
    {




        /// <summary>
        /// Retorna todos questionários validos do usuário
        /// </summary>
        /// <returns></returns>
        [Route("firm/{firmId:int}/user/{userId:int}")]
        [HttpGet]
        public HttpResponseMessage GetQuiz(int firmId, int userId)
        {
            HttpResponseMessage result = null;
            try
            {
                if (firmId > 0)
                {
                    var quiz = QuizService.Instance.GetQuiz(firmId, userId);
                    result = Request.CreateResponse(HttpStatusCode.OK, quiz);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }

            catch (Exception ex)
            {
                ServiceHelper.ThrowError(ex);
            }

            return result;
        }



        [Route("")]
        [HttpPost]
        public HttpResponseMessage AnswerQuestion(Model.Firm.Domain.QuestionAnsweredDTO to)
        {

            HttpResponseMessage result = null;
            try
            {
                var item = QuestionAnsweredService.Instance.AnswerQuestion(to);
                    
                    result = Request.CreateResponse(HttpStatusCode.OK, to);
            }

            catch (Exception ex)
            {
                ServiceHelper.ThrowError(ex);
            }

            return result;
        }

    }
}