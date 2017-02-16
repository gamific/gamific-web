using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Web;

namespace Vlast.Gamific.Api.Account.Dto
{
    /// <summary>
    /// Dados para alteração da senha do usuário
    /// </summary>
    [DataContract]
    public class ResetPwdRequest
    {
        [DataMember(Name = "userName")]
        public string UserName { get; set; }
 
    }
}