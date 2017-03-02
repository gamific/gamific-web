using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Model.School.DTO;
using Vlast.Gamific.Model.Account.DTO;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Web.Services.Engine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Vlast.Gamific.Web.Controllers.Account
{
    [Authorize]
    [RoutePrefix("acesso")]
    public class AccountController : BaseController
    {

        [Route("login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Route("resetarSenha")]
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }


        [Route("logoff")]
        public ActionResult LogOff(string url)
        {
            Session.Clear();
            AuthenticationManager.SignOut();
            return Redirect("/acesso/login");
        }


        [Route("resetarSenha")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(LoginViewModel model)
        {
            AuthResult result = new AuthResult();

            result = AccountHandler.ResetPassword(new LoginRequest() { Email = model.Email, UserName = model.Email });

            if (result.AuthStatus == AuthStatus.OK)
            {
                Success("Nova senha gerada com sucesso ! Confirme seu e-mail para receber a nova senha.");
                return View(); ;
            }

            switch (result.AuthStatus)
            {
                default:
                    ModelState.AddModelError("", "Erro ao resetar a sua senha. Contate o suporte técnico.");
                    return View(model);
            }
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
                        new Error
                        {
                            error = "Erro ao resetar a sua senha. Contate o suporte técnico."
                        },
                        Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                      );

                    return json;
            }
        }

        class Error
        {
            public string error { get; set; }
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

            if(result.AuthStatus == AuthStatus.OK)
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
                    new Error {
                        error = result.AuthStatus.ToString()
                    },
                    Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                  );

            return json;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = "/admin/empresas")
        {

            AuthStatus result = LoginGeral(model);

            switch (result)
            {
                case AuthStatus.OK:
                    return Redirect(returnUrl);
                case AuthStatus.USER_BLOQUED:
                    ModelState.AddModelError("", "Este usuário está bloqueado. Contate o suporte técnico.");
                    return View(model);
                case AuthStatus.USER_CANCELED:
                case AuthStatus.USER_NOT_EXISTS:
                    ModelState.AddModelError("", "O usuário informado nao existe.");
                    return View(model);
                case AuthStatus.INVALID_USERNAME:
                case AuthStatus.INVALID_PASSWORD:
                    ModelState.AddModelError("", "Usuário ou senha estão incorretos.");
                    return View(model);
                default:
                    ModelState.AddModelError("", "Erro ao efetuar o login. Contate o suporte técnico.");
                    return View(model);
            }

        }

        [ValidateAntiForgeryToken]
        private AuthStatus LoginGeral(LoginViewModel model)
        {
            AuthResult result = new AuthResult();

            if (!ModelState.IsValid)
            {
                return AuthStatus.ERROR;
            }

            result = AccountHandler.Login(new LoginRequest()
            {
                UserName = model.Email,
                Password = model.Password,
                TokenMobile = model.tokenMobile,
                Device = model.tipoDispositivo
            });

            if (result.AuthStatus == AuthStatus.OK)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Sid, result.UserId.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, result.UserId.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, model.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()));
                claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "http://vlast.com.br"));


                bool isSystemAdmin = false;
                foreach (var role in result.UserRoles)
                {
                    if (role == Roles.ADMINISTRATOR)
                    {
                        isSystemAdmin = true;
                    }

                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }


                if (!isSystemAdmin)
                {
                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId(result.UserId);
                    WorkerTypeEntity profile = WorkerTypeRepository.Instance.GetById(worker.WorkerTypeId);
                    claims.Add(new Claim(ClaimTypes.Role, profile.ProfileName.ToString()));
                }

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignIn(identity);

                return AuthStatus.OK;
            }

            return AuthStatus.ERROR;
        }


        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }




    
    /// <summary>
    /// Classe para tratamento de eventos ocorridos no login e criação de usuários
    /// </summary>
    public class AccountHelper
    {
        /// <summary>
        /// Mapeia os resultados de autenticação do framework Asp.net Identity
        /// para o AuthResult da app.
        /// </summary>
        /// <param name="userResult"></param>
        /// <returns></returns>
        public static GenericResult MapFrameworkAuthenticationResult(IdentityResult userResult)
        {
            GenericResult auth = new GenericResult();

            if (userResult.Succeeded)
            {
                auth.AuthStatus = AuthStatus.OK;
            }
            else if (userResult.Errors != null && userResult.Errors.Count() > 0)
            {
                string error = userResult.Errors.FirstOrDefault();
                if (error.Contains("Email") && error.Contains("already taken"))
                {
                    auth.AuthStatus = AuthStatus.USER_ALREADY_EXISTS;
                }
                else if (error.Contains("Name") && error.Contains("already taken"))
                {
                    auth.AuthStatus = AuthStatus.USER_ALREADY_EXISTS;
                }
                else if (error.Contains("cannot be null or empty")
                    || error.Contains("is invalid")
                    || error.Contains("only contain letters or digits"))
                {
                    auth.AuthStatus = AuthStatus.INVALID_USERNAME;
                }
                else if (error.Contains("Passwords must have at least one uppercase"))
                {
                    auth.AuthStatus = AuthStatus.INVALID_PASSWORD;
                }
                else
                {
                    auth.AuthStatus = AuthStatus.ERROR;
                }
            }

            return auth;
        }

        /// <summary>
        /// Tratamento dos erros ocorridos na criação ou edição dos usuários
        /// </summary>
        /// <param name="result"></param>
        public static string HandleError(GenericResult result)
        {
            if (result.AuthStatus == AuthStatus.USER_ALREADY_EXISTS)
            {
                return "Já existe um usuário com este email.";
            }
            else if (result.AuthStatus == AuthStatus.INVALID_PASSWORD)
            {
                return "Senha incorreta. Utilize letras e números.";
            }
            else if (result.AuthStatus == AuthStatus.INVALID_USERNAME)
            {
                return "Nome de usuário incorreto.";
            }
            else if (result.AuthStatus == AuthStatus.ERROR)
            {
                return "Erro inesperado.";
            }
            else if (result.AuthStatus == AuthStatus.INVALID_CPF)
            {
                return "Cpf Invalido.";
            }
            else
            {
                return "Erro inesperado.";
            }
        }
    }
}
