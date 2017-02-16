
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Vlast.Gamific.Web.Models;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Util.Instrumentation;
using Vlast.Util.Parameter;

namespace Vlast.Gamific.Services.Account
{
    public class JWTHandler : DelegatingHandler
    {

        private static DateTime _epochDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static InMemorySymmetricSecurityKey _securityKey;
        private static JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private static string ACCESS_TOKEN_KEY = "access_token";
        private static string TOKEN_ISSUER = "vlast";
        private static string TOKEN_AUDIENCE = "http://vlast.com.br";
        private static int TOKEN_EXPIRATION_HOURS = 10000;

        #region Criação e validação de tokens

        /// <summary>
        /// Cria um token a partir de um usuário.
        /// </summary>
        /// <param name="authResult"></param>
        internal static AuthResult CreateToken(AuthResult authResult)
        {
          
            SecurityTokenDescriptor descriptor =  JWTHandler.CreateSecurityTokenDescriptor(authResult);
            SecurityToken token = _tokenHandler.CreateToken(descriptor);

            authResult.ExpiresIn = Convert.ToInt64((token.ValidTo - _epochDate).TotalMilliseconds);
            authResult.AcessToken = _tokenHandler.WriteToken(token);
            authResult.TokenType = "JWT";
            return authResult;
        }


        /// <summary>
        /// Cria a estrutura do token.
        /// </summary>
        /// <param name="authResult"></param>
        /// <returns></returns>
        internal static SecurityTokenDescriptor  CreateSecurityTokenDescriptor(AuthResult authResult)
        {
            if (_securityKey == null)
            {
                _securityKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(ParameterCache.Get("TOKEN_SYMETRIC_KEY")));
            }

           
            int maxRole = authResult.UserRoles.Max(r => (int)r);

            var claimList = new List<System.Security.Claims.Claim>()
                    {
                        new System.Security.Claims.Claim(ClaimTypes.Sid,  authResult.UserId.ToString()),
                        new System.Security.Claims.Claim(ClaimTypes.Role, maxRole.ToString())
                    };

            string smymetricKey = ParameterCache.Get("TOKEN_SYMETRIC_KEY");

            var now = DateTime.UtcNow;
            System.Security.Claims.Claim[] claims = claimList.ToArray();
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = TOKEN_ISSUER,
                AppliesToAddress = TOKEN_AUDIENCE,
                Lifetime = new Lifetime(now, now.AddHours(TOKEN_EXPIRATION_HOURS)),
                SigningCredentials = new SigningCredentials(_securityKey,
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),
            };
        }


        /// <summary>
        /// Valida um token.
        /// </summary>
        /// <returns></returns>
        internal static AuthResult ValidateToken()
        {
            if (_securityKey == null)
            {
                _securityKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(ParameterCache.Get("TOKEN_SYMETRIC_KEY")));
            }

            AuthResult result = new AuthResult();
            string accessToken = ReadTokenFromRequest();

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = TOKEN_AUDIENCE,
                    ValidIssuer = TOKEN_ISSUER,
                    IssuerSigningKey = _securityKey
                };

                SecurityToken jwtToken = null;
                try
                {
                    result.TokenClaims = _tokenHandler.ValidateToken(accessToken, validationParameters, out jwtToken);
                }
                catch (SecurityTokenExpiredException)
                {
                    result.AuthStatus = AuthStatus.TOKEN_EXPIRED;
                }
                catch
                {
                    result.AuthStatus = AuthStatus.UNAUTHORIZED;
                }
            }
            else
            {
                result.AuthStatus = AuthStatus.ANONYMOUS;
            }

            return result;
        }

        /// <summary>
        /// Try reads access_token from request.
        /// </summary>
        /// <returns></returns>
        private static string ReadTokenFromRequest()
        {
            string token = "";
            try
            {
                token = HttpContext.Current.Request.QueryString.Get(ACCESS_TOKEN_KEY);

                if (string.IsNullOrWhiteSpace(token))
                {
                    token = HttpContext.Current.Request.Headers[ACCESS_TOKEN_KEY];
                }
            }
            catch
            {
            }

            return token;
        }

        #endregion
    }

    /// <summary>
    /// Autorização customizada sa API
    /// </summary>
    public class JWTAuthorize : AuthorizeAttribute
    {

        /// <summary>
        /// Validação da segurança dos webservices
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            JWTAuthorize autorize = actionContext.ActionDescriptor.GetCustomAttributes<JWTAuthorize>().FirstOrDefault();

            AuthResult result = JWTHandler.ValidateToken();

            if (result.AuthStatus == AuthStatus.OK)
            {
                List<Claim> roleClaims = new List<Claim>();

                //Recupera as roles do usuário
                string roles = result.TokenClaims.Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(r => r.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(roles))
                {
                    string[] splitedRoles = roles.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string role in splitedRoles)
                    {
                        roleClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                //Adiciona as roles separadas
                ClaimsIdentity identity = new ClaimsIdentity(result.TokenClaims.Identity, roleClaims, result.TokenClaims.Identity.AuthenticationType, ClaimTypes.Name, ClaimTypes.Role);

                ClaimsPrincipal mappedPrincipal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = mappedPrincipal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = mappedPrincipal;
                }

            }
            else if (result.AuthStatus != AuthStatus.ANONYMOUS)
            {
                HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, result);
                response.Headers.Add("WWW-Authenticate", "xBasic realm=\"\"");
                throw new HttpResponseException(response);
            }
            else if (result.AuthStatus == AuthStatus.ANONYMOUS)
            {
                if (autorize != null)
                {

                    HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, result);
                    response.Headers.Add("WWW-Authenticate", "xBasic realm=\"\"");
                    throw new HttpResponseException(response);
                }
            }

            return base.IsAuthorized(actionContext);
        }

    }
}