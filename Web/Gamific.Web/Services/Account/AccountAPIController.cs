using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Services.Account;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Api.Account
{
    /// <summary>
    /// Autenticação e criação de usuários
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/account")]
    public class AccountAPIController : ApiController
    {

        /// <summary>
        /// Efetua o login do usuário, retornando o token que será utilizado para chamdas a serviços
        /// autenticados.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns>AccessToken, OK; UNAUTHORIZED; INVALID_CREDENTIALS;</returns>
        [Route("login")]
        [HttpPost]
        public AuthResult Login(LoginRequest loginRequest)
        {
            AuthResult authResult = new AuthResult();

            try
            {
                authResult = AccountHandler.Login(loginRequest);
                if (authResult.AuthStatus == AuthStatus.OK)
                {
                    authResult = JWTHandler.CreateToken(authResult);
                }
            }

            catch (Exception ex)
            {
                Logger.LogException(ex);
                authResult.AuthStatus = AuthStatus.ERROR;
            }

            if (authResult.AuthStatus != AuthStatus.OK)
            {
                ServiceHelper.ThrowBadRequest<AuthResult>(Request, authResult);
            }


            return authResult;
        }


        /// <summary>
        /// Cria um novo usuário 
        /// </summary>
        /// <param name="newRequest"></param>
        /// <returns></returns>
        [Route("subscribe")]
        [HttpPost]
        public AuthResult New(NewRequest newRequest)
        {
            AuthResult authResult = null;

            try
            {
                authResult = AccountHandler.CreateUser(newRequest);
                if (authResult.AuthStatus == AuthStatus.OK)
                {
                    authResult = this.Login(new LoginRequest() { UserName = newRequest.Email, Password = newRequest.Password });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                authResult.AuthStatus = AuthStatus.ERROR;
            }

            if (authResult.AuthStatus != AuthStatus.OK)
            {
                ServiceHelper.ThrowBadRequest<AuthResult>(Request, authResult);
            }

            return authResult;
        }

        /// <summary>
        /// Alteração da senha atual do usuário
        /// </summary>
        /// <param name="changePwdRequest"></param>
        /// <returns></returns>
        [Route("changepwd")]
        [JWTAuthorize]
        [HttpPost]
        public AuthResult ChangePassword(ChangePwdRequest changePwdRequest)
        {
            AuthResult authResult = null;

            try
            {
                int userId = ServiceHelper.CurrentUserId;
                authResult = AccountHandler.ChangePassword(userId, changePwdRequest.CurrentPwd, changePwdRequest.NewPassword);

                if (authResult.AuthStatus == AuthStatus.OK)
                {
                    authResult = JWTHandler.CreateToken(authResult);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                authResult.AuthStatus = AuthStatus.ERROR;
            }

            if (authResult.AuthStatus != AuthStatus.OK)
            {
                ServiceHelper.ThrowBadRequest<AuthResult>(Request, authResult);
            }

            return authResult;
        }


        /// <summary>
        /// Reseta a senha e envia para o usuário
        /// </summary>
        /// <param name="resetPwdRequest"></param>
        /// <returns></returns>
        [Route("resetpwd")]
        [HttpPost]
        public AuthResult ResetPassword(string email)
        {
            AuthResult result = new AuthResult();

            result = AccountHandler.ResetPassword(new LoginRequest() { Email = email, UserName = email });
            if (result.AuthStatus != AuthStatus.OK)
            {
                ServiceHelper.ThrowBadRequest<AuthResult>(Request, result);
            }
            else
            {
                result.Message = "Um e-mail com as instruções de recuperação de senha foi enviado. Verifique sua caixa de entrada.";
            }

            switch (result.AuthStatus)
            {
                default:
                    result.Message = "Erro ao resetar a sua senha. Contate o suporte técnico.";
                    return result;
            }
        }

    }
}