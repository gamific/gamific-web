using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Vlast.Util.Instrumentation
{
    /// <summary>
    /// Helper para utilização específica para serviços Rest
    /// </summary>
    public class ServiceHelper
    {

        public static int CurrentUserId
        {
            get
            {
                return Convert.ToInt32((HttpContext.Current.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            }
        }

        /// <summary>
        /// Transforma uma data para horário de verão e GMT-3.
        /// </summary>
        /// <returns></returns>
        public static DateTime AdjustToGMT3DayLight(DateTime sourceDate)
        {
            if (sourceDate.Kind == DateTimeKind.Utc || sourceDate.Kind == DateTimeKind.Unspecified)
            {
                if (sourceDate.Kind == DateTimeKind.Unspecified)
                {
                    sourceDate = DateTime.SpecifyKind(sourceDate, DateTimeKind.Utc);
                }

                sourceDate = TimeZoneInfo.ConvertTimeFromUtc(sourceDate, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            }

            return sourceDate;
        }


        public static string GetWeekDayName(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return "Domingo";
                case DayOfWeek.Monday:
                    return "Segunda-feira";
                case DayOfWeek.Tuesday:
                    return "Terça-feira";
                case DayOfWeek.Wednesday:
                    return "Quarta-feira";
                case DayOfWeek.Thursday:
                    return "Quinta-feira";
                case DayOfWeek.Friday:
                    return "Sexta-feira";
                default: return "Sábado";
            }
        }

        /// <summary>
        /// Cria um retorno com imagem nome de arquivo e cache control
        /// </summary>
        /// <param name="request"></param>
        /// <param name="mediaStream"></param>
        /// <param name="contentType">MediaTypeNames</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateCachedResponse(HttpRequestMessage request, Stream mediaStream, string contentType, string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {

                result.Content = new StreamContent(mediaStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = fileName
                };

                result.Headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromSeconds(3600) };

            }
            catch
            {
                result = new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return result;
        }

        #region Tratamento de erros


        public static void ThrowError(Exception ex, string custoMsg = "")
        {
            Logger.LogException(ex);
            ThrowError(HttpStatusCode.BadRequest, custoMsg + " ## " + ex.Message + " ## " + ex.ToString());
        }


        public static void ThrowBadRequest<T>(HttpRequestMessage request, T content)
        {
            HttpResponseMessage response = request.CreateResponse<T>(HttpStatusCode.BadRequest, content);
            throw new HttpResponseException(response);
        }

        public static void ThrowError(string message)
        {
            ThrowError(HttpStatusCode.BadRequest, message);
        }

        public static void ThrowError(HttpStatusCode httpStatus, string errorMsg)
        {
            if (!string.IsNullOrEmpty(errorMsg))
            {
                errorMsg = errorMsg.Replace("\"", "");
                errorMsg = errorMsg.Replace("'", "");
                errorMsg = errorMsg.Replace("\r", " ");
                errorMsg = errorMsg.Replace("\n", " ");
            }

            var resp = new HttpResponseMessage(httpStatus)
            {

                Content = new StringContent("{\"Error\":\"" + errorMsg + "\", \"Status\":\"" + httpStatus.ToString() + "(" + (int)httpStatus + ")" + "\" }", Encoding.UTF8, "application/json"),
                ReasonPhrase = errorMsg
            };

            Logger.LogError(errorMsg);
            throw new HttpResponseException(resp);
        }

        #endregion
    }
}
