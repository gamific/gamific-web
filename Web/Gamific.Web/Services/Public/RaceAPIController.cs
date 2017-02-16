using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Vlast.Gamific.Api.Account.Dto;
using Vlast.Gamific.Services.Account;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Web.Services.Account.Dto;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Api.Public
{
    /// <summary>
    /// Serviço de corrida
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/race")]
    public class RaceAPIController : ApiController
    {

    }
}