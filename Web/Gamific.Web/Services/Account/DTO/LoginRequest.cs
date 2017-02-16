using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Web;

namespace Vlast.Gamific.Api.Account.Dto
{
    /// <summary>
    /// Requisição de login
    /// </summary>
    [DataContract]
    public class LoginRequest
    {
        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "tokenMobile")]
        public string TokenMobile { get; set; }

        [DataMember(Name = "device")]
        public string Device { get; set; }

    }
}