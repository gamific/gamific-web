using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;
using Vlast.Broker.EMAIL;
using Vlast.Gamific.Account.Model;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Account.DTO;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Util.Global;
using Vlast.Util.Parameter;

namespace Vlast.Gamific.Web.Services.Account.BIZ
{
    /// <summary>
    /// Tratamento do login e criação de usuários
    /// </summary>
    public class AccountHandler
    {

        /// <summary>
        /// Reseta a senha de um usuário
        /// </summary>
        /// <param name="ResetPwdRequest"></param>
        /// <returns>Token de autenticação em caso de sucesso.</returns>
        public static AuthResult ResetPassword(LoginRequest ResetPwdRequest)
        {
            AuthResult authResult = new AuthResult();
            try
            {
                UserAccountEntity user = AccountRepository.Instance.FindByUserName(ResetPwdRequest.UserName != null ? ResetPwdRequest.UserName.ToLower() : null);

                if (user == null)
                {
                    authResult.AuthStatus = AuthStatus.USER_NOT_EXISTS;
                    return authResult;
                }

                UserProfileEntity profile = AccountRepository.Instance.GetProfileById(user.Id);

                if (profile == null)
                {
                    authResult.AuthStatus = AuthStatus.USER_NOT_EXISTS;
                    return authResult;
                }

                Random rnd = new Random();

                string newPwd = rnd.Next().ToString(); //"PWD" + rnd.Next();

                NewRequest request = new NewRequest();

                request.Email = user.UserName;
                request.Username = user.UserName;
                request.Password = newPwd;

                UpdateUser(request);

                string EmailBody = "Olá! sua nova senha para acessar a plataforma do Gamific é: " + newPwd;

                string EmailTo = ParameterCache.Get("RESET_PASSWORD_EMAIL");

                var result = EmailDispatcher.SendEmail(EmailTo, "Nova Senha Gamific", new List<string>() { user.UserName }, EmailBody);

                if (result)
                {
                    authResult.AuthStatus = AuthStatus.OK;
                }
                else
                {
                    authResult.AuthStatus = AuthStatus.ERROR;
                }

            }
            catch (Exception ex)
            {
                authResult.AuthStatus = AuthStatus.ERROR;
            }

            return authResult;
        }

        /// <summary>
        /// Tenta efetuar o login com o usuário e senha fornecidos.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns>Token de autenticação em caso de sucesso.</returns>
        public static AuthResult Login(LoginRequest loginRequest)
        {
            AuthResult authResult = new AuthResult();
            try
            {
                UserAccountEntity user = AccountRepository.Instance.FindByUserName(loginRequest.UserName != null ? loginRequest.UserName.ToLower() : null);

                if (user == null)
                {
                    authResult.AuthStatus = AuthStatus.USER_NOT_EXISTS;
                    return authResult;
                }

                //Valida senha
                bool isValidPassword = PasswordUtils.ValidatePassword(loginRequest.Password, user.SecurityStamp, user.PasswordHash);

                //Senha inválida
                if (!isValidPassword)
                {
                    authResult.AuthStatus = AuthStatus.INVALID_CREDENTIALS;
                    return authResult;
                }

                user.LastLogin = DateTime.UtcNow;
                user.TokenMobile = loginRequest.TokenMobile != null ? loginRequest.TokenMobile : user.TokenMobile;
                user.Device = loginRequest.Device != null ? loginRequest.Device : user.Device;
                AccountRepository.Instance.Update(user);

                UserProfileEntity profile = AccountRepository.Instance.GetProfileById(user.Id);
                if (profile == null)
                {
                    authResult.AuthStatus = AuthStatus.USER_NOT_EXISTS;
                    return authResult;
                }

                authResult.UserRoles = AccountRepository.Instance.GetUserRoles(user.Id);

                authResult.UserId = user.Id;
                authResult.AuthStatus = AuthStatus.OK;
            }
            catch (Exception ex)
            {
                authResult.AuthStatus = AuthStatus.ERROR;
            }

            return authResult;
        }

        /// <summary>
        /// Atualiza um usuário.
        /// </summary>
        /// <param name="newUserRequest"></param>
        /// <returns>Token de autenticação em caso de sucesso.</returns>
        public static AuthResult UpdateUser(NewRequest newUserRequest)
        {
            AuthResult authResult = ValidateRequest(newUserRequest, true);

            if (authResult.AuthStatus == AuthStatus.OK)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    UserAccountEntity user = AccountRepository.Instance.FindByUserName(newUserRequest.Username);

                    if (!String.IsNullOrWhiteSpace(newUserRequest.Password))
                    {
                        var security = PasswordUtils.CreateHash(newUserRequest.Password);
                        user.PasswordHash = security.Item2;
                        user.SecurityStamp = security.Item1;
                    }
                    else
                    {
                        user.PasswordHash = user.PasswordHash;
                        user.SecurityStamp = user.SecurityStamp;
                    }

                    if (!String.IsNullOrWhiteSpace(newUserRequest.Email))
                    {
                        user.UserName = newUserRequest.Email;
                    }

                    AccountRepository.Instance.Update(user);

                    scope.Complete();
                }
            }

            return authResult;
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="newUserRequest"></param>
        /// <returns>Token de autenticação em caso de sucesso.</returns>
        public static AuthResult CreateUser(NewRequest newUserRequest)
        {
            AuthResult authResult = ValidateRequest(newUserRequest, true);

            if (authResult.AuthStatus == AuthStatus.OK)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (!string.IsNullOrWhiteSpace(newUserRequest.Cpf))
                    {
                        newUserRequest.Cpf = newUserRequest.Cpf.Replace(".", "").Replace("-", "");
                    }
                    UserAccountEntity user = AccountRepository.Instance.FindByUserName(newUserRequest.Email);
                    UserProfileEntity profile = AccountRepository.Instance.GetProfileByCPF(newUserRequest.Cpf);
                    if (user != null)
                    {
                        authResult.AuthStatus = AuthStatus.USER_ALREADY_EXISTS;
                    }
                    else
                    {
                        var security = PasswordUtils.CreateHash(newUserRequest.Password);
                        var userAccount = AccountRepository.Instance.Create(new UserAccountEntity()
                        {
                            UserName = newUserRequest.Email.ToLower(),
                            PasswordHash = security.Item2,
                            SecurityStamp = security.Item1,
                        });

                        if (userAccount.Id > 0)
                        {
                            UserProfileEntity profileData = new UserProfileEntity()
                            {
                                CPF = newUserRequest.Cpf,
                                Name = newUserRequest.Name,
                                Phone = newUserRequest.Phone,
                                Id = userAccount.Id
                            };

                            UserProfileEntity userProfile = AccountRepository.Instance.CreateProfile(profileData);

                            AccountRepository.Instance.CreateUserRole(userAccount.Id, Roles.WORKER);

                            authResult.UserId = userAccount.Id;
                        }

                        scope.Complete();
                    }
                }
            }

            return authResult;
        }

        /// <summary>
        /// Altera a senha de um usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public static AuthResult ChangePassword(int userId, string currentPwd, string newPwd)
        {
            AuthResult authResult = new AuthResult();

            UserAccountEntity user = AccountRepository.Instance.GetById(userId);

            if (user == null)
            {
                authResult.AuthStatus = AuthStatus.USER_NOT_EXISTS;
                return authResult;
            }

            //Valida senha
            bool isValidPassword = PasswordUtils.ValidatePassword(currentPwd, user.SecurityStamp, user.PasswordHash);

            //Senha inválida
            if (!isValidPassword)
            {
                authResult.AuthStatus = AuthStatus.INVALID_CREDENTIALS;
                return authResult;
            }

            var security = PasswordUtils.CreateHash(newPwd);
            user.PasswordHash = security.Item2;
            user.SecurityStamp = security.Item1;

            AccountRepository.Instance.Update(user);

            authResult.UserRoles = AccountRepository.Instance.GetUserRoles(user.Id);


            return authResult;
        }

        /// <summary>
        /// criaçao de um novo usuário com um perfil
        /// </summary>
        /// <param name="newUserRequest"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static AuthResult CreateFirmUser(NewRequest newUserRequest, Roles role)
        {
            AuthResult authResult = ValidateRequest(newUserRequest, true);

            if (authResult.AuthStatus == AuthStatus.OK)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    
                    if (!string.IsNullOrWhiteSpace(newUserRequest.Cpf))
                    {
                        newUserRequest.Cpf = newUserRequest.Cpf.Replace(".", "").Replace("-", "");
                    }
                    
                    UserAccountEntity user = AccountRepository.Instance.FindByUserName(newUserRequest.Username);

                    if (user != null)
                    {
                        authResult.AuthStatus = AuthStatus.USER_ALREADY_EXISTS;
                    }
                    else
                    {

                        var security = PasswordUtils.CreateHash(newUserRequest.Password);
                        var userAccount = AccountRepository.Instance.Create(new UserAccountEntity()
                        {
                            UserName = newUserRequest.Username.ToLower(),
                            PasswordHash = security.Item2,
                            SecurityStamp = security.Item1,
                        });

                        if (userAccount.Id > 0)
                        {
                            UserProfileEntity profileData = new UserProfileEntity()
                            {
                                CPF = newUserRequest.Cpf,
                                Name = newUserRequest.Name,
                                Phone = newUserRequest.Phone,
                                Id = userAccount.Id,
                                Email = newUserRequest.Email,
                            };

                            UserProfileEntity userProfile = AccountRepository.Instance.CreateProfile(profileData);

                            AccountRepository.Instance.CreateUserRole(userAccount.Id, role);

                            authResult.UserId = userAccount.Id;
                        }

                        scope.Complete();
                    }
                }
            }

            return authResult;
        }

        /// <summary>
        /// Valida os parametros informados para criação de usuário.
        /// </summary>
        /// <param name="newUserRequest"></param>
        ///   <returns></returns>
        private static AuthResult ValidateRequest(NewRequest newUserRequest, bool create = true)
        {
            AuthResult authResult = new AuthResult();

            if (TextUtils.StringContainsAccents(newUserRequest.Username))
            {
                authResult.AuthStatus = AuthStatus.INVALID_USERNAME;
            }

            authResult.AuthStatus = ValidatePassword(newUserRequest.Password) ? AuthStatus.OK : AuthStatus.INVALID_PASSWORD;

            if (authResult.AuthStatus == AuthStatus.OK && create)
            {
                if (!string.IsNullOrWhiteSpace(newUserRequest.Cpf))
                {
                    if (!TextUtils.IsValidCpf(newUserRequest.Cpf))
                    {
                        authResult.AuthStatus = AuthStatus.INVALID_CPF;
                    }
                }
               
            }

            return authResult;
        }

        /// <summary>
        /// Validar senha de criação / alteração
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static bool ValidatePassword(string password)
        {
            Regex number = new Regex("[0-9]");

            if (password.Length < 6)
                return false;
            else if (number.Matches(password).Count < 1)
                return false;

            return true;
        }

    }
}
