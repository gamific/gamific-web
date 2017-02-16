using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class EpisodeEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile EpisodeEngineService instance;

        private EpisodeEngineService() : base(ENGINE_API + "episode/") { }

        public static EpisodeEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new EpisodeEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public EpisodeEngineDTO CreateOrUpdate(EpisodeEngineDTO episode)
        {
            return PostDTO<EpisodeEngineDTO>(ref episode);
        }

        public EpisodeEngineDTO GetById(string episodeId)
        {
            return GetDTO<EpisodeEngineDTO>(episodeId);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public void CloseById(string id)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.UploadString(ENGINE_API + "closeEpisode?episodeId=" + id, "POST");


                }
            }
            catch (Exception e)
            {

            }
        }

        public GetAllDTO GetByGameIdAndActiveIsTrue(string gameId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "search/findByGameIdAndActiveIsTrue/" + "?gameId=" + gameId + "&page=" + pageIndex + "&size=" + pageSize);
                    return JsonConvert.DeserializeObject<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public EpisodeEngineDTO Clone(string name, string id)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.UploadString(ENGINE_API + "cloneEpisode?name=" + name + "&episodeId=" + id, "POST");

                    return JsonConvert.DeserializeObject<EpisodeEngineDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO FindByGameIdAndActive(string gameId, int isActive, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(path + "search/findByGameIdAndActive" + "?size=" + pageSize + "&page=" + pageIndex + "&gameId=" + gameId + "&active=" + isActive);

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


        public GetAllDTO GetByGameIdAndActive(string gameId, int active)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "search/findByGameIdAndActive/" + "?gameId=" + gameId + "&active=" + active);

                    return JsonConvert.DeserializeObject<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetByGameId(string gameId, int pageIndex = 0, int pageSize = 10 )
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "search/findByGameId/" + "?gameId=" + gameId + "&page=" + pageIndex + "&size=" + pageSize);

                    return JsonConvert.DeserializeObject<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO resultsByEpisodeIdAndMetricId(string episodeId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(ENGINE_API + "resultsByEpisodeIdAndMetricId" + "?episodeId=" + episodeId + "&metricId=" + metricId + "&page=" + pageIndex + "&size=" + pageSize);

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

        public List<EpisodeEngineDTO> EpisodesByPlayerId(string playerId, string gameId, int active)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(ENGINE_API + "episodeByPlayerId" + "?playerId=" + playerId + "&gameId=" + gameId + "&active=" + active);

                    return JsonConvert.DeserializeObject<List<EpisodeEngineDTO>>(response,
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

        public long GetCountPlayersByEpisodeId(string episodeId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "countPlayersByEpisodeId?episodeId=" + episodeId);

                    return JsonConvert.DeserializeObject<long>(responce,
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

        #endregion
    }

}