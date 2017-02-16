using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Claims;
using Vlast.Gamific.Model.Account.DTO;

namespace Vlast.Gamific.Web.Services.Account.Dto
{
    [DataContract]
    public class GenericResult
    {
        [DataMember(Name = "auth_status_code", EmitDefaultValue = false)]
        public AuthStatus AuthStatus { get; set; }

        [DataMember(Name = "auth_status", EmitDefaultValue = false)]
        public string AuthStatusValue
        {
            get { return this.AuthStatus.ToString(); }
            set { this.AuthStatus = (AuthStatus)Enum.Parse(typeof(AuthStatus), value); }
        }

        public GenericResult()
        {
            AuthStatus = AuthStatus.OK;
        }
    }

    /// <summary>
    /// Resultado do login
    /// </summary>
    [DataContract]
    public class AuthResult : GenericResult
    {
        [DataMember(Name = "access_token", EmitDefaultValue = false)]
        public string AcessToken { get; set; }

        [DataMember(Name = "expires_in", EmitDefaultValue = false)]
        public long ExpiresIn { get; set; }

        [DataMember(Name = "token_type", EmitDefaultValue = false)]
        public string TokenType { get; set; }

        [IgnoreDataMember]
        internal ClaimsPrincipal TokenClaims { get; set; }

        [DataMember(Name = "roles", EmitDefaultValue = false)]
        public List<Roles> UserRoles { get; set; }

        [IgnoreDataMember]
        public int UserId { get; set; }

        [IgnoreDataMember]
        public string Message { get; set; }

        public AuthResult()
        {
            AuthStatus = AuthStatus.OK;
        }
    }

    /// <summary>
    /// Status de autenticação, validação de token e criação de usuários
    /// </summary>
    [DataContract]
    public enum AuthStatus
    {
        [EnumMember]
        OK,
        [EnumMember]
        INVALID_CREDENTIALS,
        [EnumMember]
        PASSWORD_DOES_NOT_MATCH,
        [EnumMember]
        INVALID_USERNAME,
        [EnumMember]
        INVALID_PASSWORD,
        [EnumMember]
        INVALID_CPF,
        [EnumMember]
        INVALID_NAME,
        [EnumMember]
        INVALID_PHONE,
        [EnumMember]
        PENDING_CONFIRMATION,
        [EnumMember]
        USER_ALREADY_EXISTS,
        [EnumMember]
        UNAUTHORIZED,
        [EnumMember]
        TOKEN_EXPIRED,
        [EnumMember]
        ANONYMOUS,
        [EnumMember]
        USER_NOT_EXISTS,
        [EnumMember]
        INVALID_ACTIVATION_CODE,
        [EnumMember]
        ERROR,
        [EnumMember]
        INVALID_ROLE,
        [EnumMember]
        USER_BLOQUED,
        [EnumMember]
        USER_CANCELED
    }
}
