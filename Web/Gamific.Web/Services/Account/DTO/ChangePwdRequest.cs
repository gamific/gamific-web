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
    public class ChangePwdRequest
    {
        
        [DataMember(Name = "currentPwd")]
        public string CurrentPwd { get; set; }

        [DataMember(Name = "newPassword")]
        public string NewPassword { get; set; }
        
    }
}