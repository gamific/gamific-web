using System;
using Vlast.Gamific.Web.Services.Engine.DTO;
using System.Net;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Security.Claims;
using System.Linq;

namespace Vlast.Gamific.Web.Services.Engine
{
    public abstract class EngineServiceBase
    {
        #region API's URLs

        protected static string ENGINE_API { get; } = WebConfigurationManager.AppSettings["ENGINE_URL"];
        protected string path { get; }

        #endregion

        private string GetEncodedEmail
        {
            get
            {
                var userIdentity = System.Web.HttpContext.Current.User;
                if (userIdentity.Identity.IsAuthenticated)
                {
                    string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").
                        GetBytes((userIdentity.Identity as ClaimsIdentity).
                        Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value + ":" + ""));
                    return encoded;
                }
                return null;
            }
        }



        protected WebClient GetClient
        {
            get
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + GetEncodedEmail;

                return client;
            }
        }

        protected string JsonSerialize<T>(ref T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                                                new JsonSerializerSettings
                                                {
                                                    NullValueHandling = NullValueHandling.Ignore
                                                });
        }

        protected T JsonDeserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json,
                                                   new JsonSerializerSettings
                                                   {
                                                       NullValueHandling = NullValueHandling.Ignore
                                                   });
        }

        protected EngineServiceBase(string path = "")
        {
            this.path = path;
        }

        protected T PostDTO<T>(ref T dto)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.UploadString(path, "POST", JsonSerialize(ref dto));
                    return JsonDeserialize<T>(response);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        protected T PostDTO<T>(ref T dto, string email)
        {
            try
            {

                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                
               string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
               client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
               string response = client.UploadString(path, "POST", JsonSerialize(ref dto));
               return JsonDeserialize<T>(response);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected T GetDTO<T>(string id) 
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(path + id);
                    return JsonDeserialize<T>(response);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        protected T GetDTO<T>(string id, string email)
        {
            try
            {
                    WebClient client = new WebClient();
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;
                    string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                    string response = client.DownloadString(path + id);
                    return JsonDeserialize<T>(response);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected string Delete(string id)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    return client.UploadString(path + id, "DELETE", "");    
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetAll(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    
                    string response = client.DownloadString(path + "?size=" + pageSize + "&page=" + pageIndex);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

      
        public GetAllDTO GetByGameId(string gameId, int size = 1000, int page = 0)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string responce = client.DownloadString(path + "search/findByGameId?gameId=" + gameId + "&size=" + size + "&page=" + page);
                    return JsonDeserialize<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}