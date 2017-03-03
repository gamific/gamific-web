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

        [Route("resetarSenhaMobile")]
        [HttpPost]
        [AllowAnonymous]
        public string ResetPasswordMobile(LoginViewModel model)
        {
            AuthResult result = new AuthResult();

            result = AccountHandler.ResetPassword(new LoginRequest() { Email = model.Email, UserName = model.Email });

            string json = "";
            if (result.AuthStatus == AuthStatus.OK)
            {
                json = JsonConvert.SerializeObject(
                   "Nova senha gerada com sucesso ! Confirme seu e-mail para receber a nova senha.",
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

    }
}