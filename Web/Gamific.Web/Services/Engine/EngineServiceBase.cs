using System;
using Vlast.Gamific.Web.Services.Engine.DTO;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Configuration;

namespace Vlast.Gamific.Web.Services.Engine
{
    public abstract class EngineServiceBase
    {
        #region API's URLs

        protected static string ENGINE_API { get; } = WebConfigurationManager.AppSettings["ENGINE_URL"];
        protected string path { get; }

        #endregion

        protected EngineServiceBase(string path = "")
        {
            this.path = path;
        }

        protected T PostDTO<T>(ref T dto)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string json = JsonConvert.SerializeObject(dto,
                                                                Formatting.None,
                                                                new JsonSerializerSettings
                                                                {
                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                });

                    string response = "";
                    response = client.UploadString(path, "POST", json);

                    return JsonConvert.DeserializeObject<T>(response);
                }
            }
            catch(Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        protected T GetDTO<T>(string id) 
        {
            if(id == null || id == "")
            {
                throw new Exception("Id nulo ou vazio.");
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(path + id);

                    return JsonConvert.DeserializeObject<T>(response,
                                                            new JsonSerializerSettings
                                                            {
                                                                NullValueHandling = NullValueHandling.Ignore
                                                            });
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        protected string Delete(string id)
        {
            if (id == null || id == "")
            {
                throw new Exception("Id nulo ou vazio.");
            }

            try
            {
                using (WebClient client = new WebClient())
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
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(path + "?size=" + pageSize + "&page=" + pageIndex);

                    return JsonConvert.DeserializeObject<GetAllDTO>(response, 
                                                                    new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetAllByGameId(string gameId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "?size=" + pageSize + "&page=" + pageIndex);

                    return JsonConvert.DeserializeObject<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetByGameId(string gameId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "search/findByGameId?gameId=" + gameId);

                    return JsonConvert.DeserializeObject<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}