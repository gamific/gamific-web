using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Account.DTO;
using Vlast.Gamific.Model.Account.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Controllers
{
    /// <summary>
    /// Controller genérico para criação de métodos
    /// e tratamento de comportamentos de proósito geral
    /// </summary>
    [HandleError()]
    public class BaseController : Controller
    {
        public class CheckBoxValue
        {
            public int Value { get; set; }
            public string  Text { get; set; }
            public bool Checked { get; set; }
        }

        protected static CultureInfo PT_BR_CULTURE = CultureInfo.GetCultureInfo("pt-BR");

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = PT_BR_CULTURE;
            Thread.CurrentThread.CurrentUICulture = PT_BR_CULTURE;

            return base.BeginExecuteCore(callback, state);
        }

        /// <summary>
        /// Id do usuário logado
        /// </summary>
        protected int CurrentUserProfileId
        {
            get
            {
                return CurrentUserId;
            }
        }

        /// <summary>
        /// Gera uma cor aleatoria
        /// </summary>
        /// <param name="factor"></param>
        /// <returns></returns>
        public string GenerateColorHexadecimal(int factor)
        {
            if(factor == 0)
                factor = 1;

            int maxNumber = (256 * 256 * 256) - 1; 
            Random random = new Random();
            string corAleatoria = "#";

            int rand = (random.Next() * factor) % maxNumber;
            corAleatoria += rand.ToString("x6");
            
            return corAleatoria;
        }

        /// <summary>
        /// Retorna o URL para o servidor
        /// </summary>
        public static string CurrentURL
        {
            get
            {
                return WebConfigurationManager.AppSettings["S3_URL"];
            }
        }
        
        /// <summary>
        /// Email do usuário logado
        /// </summary>
        public static string CurrentUserEmail
        {
            get
            {
                var userIdentity = System.Web.HttpContext.Current.User;
                if (userIdentity.Identity.IsAuthenticated)
                {
                    return (userIdentity.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                }
                return null;
            }
        }

        public static int CurrentUserId
        {
            get
            {
                var userIdentity = System.Web.HttpContext.Current.User;
                if (userIdentity.Identity.IsAuthenticated)
                {
                    return Convert.ToInt32((userIdentity.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                }
                return -1;
            }
        }


        /// <summary>
        /// Empresa associada com o colaborador logado
        /// </summary>
        public static DataEntity CurrentFirm
        {
            get
            {
                string key = "CURRENT_FIRM_" + CurrentUserId;
                if (System.Web.HttpContext.Current.Session[key] == null)
                {
                    DataEntity firm = DataRepository.Instance.GetWorkerFirm(CurrentUserId);
                    if (firm != null)
                    {
                        System.Web.HttpContext.Current.Session.Add(key, firm);
                    }
                }
                
                return (DataEntity)System.Web.HttpContext.Current.Session[key];
            }
        }

        public static WorkerEntity CurrentWorker
        {
            get
            {
                string key = "CURRENT_WORKER_" + CurrentUserId;
                if (System.Web.HttpContext.Current.Session[key] == null)
                {
                    WorkerEntity worker = WorkerRepository.Instance.GetByUserId(CurrentUserId);
                    if (worker != null)
                    {
                        System.Web.HttpContext.Current.Session.Add(key, worker);
                    }
                }
                return (WorkerEntity)System.Web.HttpContext.Current.Session[key];
            }
        }

        public static UserProfileEntity CurrentUserProfile
        {
            get
            {
                string key = "CURRENT_USER_PROFILE_" + CurrentUserId;
                if (System.Web.HttpContext.Current.Session[key] == null)
                {
                    UserProfileEntity userProfile = UserProfileRepository.Instance.GetById(CurrentUserId);
                    if (userProfile != null)
                    {
                        System.Web.HttpContext.Current.Session.Add(key, userProfile);
                    }
                }
                return (UserProfileEntity)System.Web.HttpContext.Current.Session[key];
            }
        }

        public static WorkerTypeEntity CurrentWorkerType
        {
            get
            {
                WorkerTypeEntity workerType = WorkerTypeRepository.Instance.GetById(CurrentWorker.WorkerTypeId);

                string key = "CURRENT_WORKER_TYPE_" + workerType;
                if (System.Web.HttpContext.Current.Session[key] == null)
                {
                    if (workerType != null)
                    {
                        System.Web.HttpContext.Current.Session.Add(key, workerType);
                    }
                }
                return (WorkerTypeEntity)System.Web.HttpContext.Current.Session[key];
            }
        }
        /*
        public static string CurrentURL
        {
            get
            {
                return System.Web.HttpContext.Current.Request.Url.ToString();
            }
        }
        */
        public static bool IsSystemAdmin
        {
            get
            {
                var userIdentity = System.Web.HttpContext.Current.User;
                if (userIdentity.Identity.IsAuthenticated)
                {
                    return (userIdentity.Identity as ClaimsIdentity).Claims.Where(c => c.Type == ClaimTypes.Role && c.Value == Roles.ADMINISTRATOR.ToString()).Count() > 0;
                }

                return false;
            }
        }

        /// <summary>
        /// Mensagem de sucesso em operações
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dismissable"></param>
        public void Success(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        /// <summary>
        /// Mensagem de informações gerais
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dismissable"></param>
        public void Information(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        /// <summary>
        /// Mensagens de aviso
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dismissable"></param>
        public void Warning(string message, bool dismissable = true)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        /// <summary>
        /// Algum Erro encontrado
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dismissable"></param>
        public void Error(string message, Exception ex = null, bool dismissable = true)
        {

            if (ex != null)
            {
                Logger.LogException(ex);
            }

            AddAlert(AlertStyles.Danger, message, dismissable, ex);
        }


        private void AddAlert(string alertStyle, string message, bool dismissable, Exception ex = null)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable,
                ExceptionError = ex
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Transforma a data para o time zone de são paulo
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        protected DateTime ToSaoPauloTime(DateTime time)
        {
            //return time;
            return TimeZoneInfo.ConvertTimeFromUtc(time, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public static string ToGMT3(DateTime? time)
        {
            if (!time.HasValue)
                return null;
            //return  time.Value.ToString("dd/MM/yyyy HH:mm");
            return TimeZoneInfo.ConvertTimeFromUtc(time.Value, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString("dd/MM/yyyy HH:mm");
        }
    }

    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }

        public Exception ExceptionError { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }

    /// <summary>
    /// Serialização utulziando Datacontract do .net
    /// </summary>
    public class DataContractResult : JsonResult
    {
        public DataContractResult()
        {

        }

        public override void ExecuteResult(ControllerContext context)
        {
            var serializer = new DataContractJsonSerializer(this.Data.GetType());
            context.HttpContext.Response.ContentType = "application/json";
            serializer.WriteObject(context.HttpContext.Response.OutputStream,
                this.Data);
        }
    }

    /// <summary>
    /// Autorização customizada
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private static string AUTHORIZATION_STATUS_KEY = "AUTHORIZATION_STATUS_KEY";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session[AUTHORIZATION_STATUS_KEY] != null)
            {
                httpContext.Session[AUTHORIZATION_STATUS_KEY] = null;
            }

            var isAuthorized = base.AuthorizeCore(httpContext);
            if (isAuthorized)
            {
                httpContext.Session[AUTHORIZATION_STATUS_KEY] = "AUTHORIZED";
            }
            else
            {
                if (((httpContext.User).Identity).IsAuthenticated)
                    httpContext.Session[AUTHORIZATION_STATUS_KEY] = "UNAUTHORIZED";
            }

            return isAuthorized;
        }

    }


}