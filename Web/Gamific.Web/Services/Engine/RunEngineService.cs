using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class RunEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile RunEngineService instance;

        private RunEngineService() : base(ENGINE_API + "run/") { }

        public static RunEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new RunEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public RunEngineDTO CreateOrUpdate(RunEngineDTO run)
        {
            return PostDTO<RunEngineDTO>(ref run);
        }

        public GetAllDTO GetRunsByTeamId(string teamId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "search/findByTeamId/?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex);

                    return JsonConvert.DeserializeObject<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RunEngineDTO GetById(string runId)
        {
            return GetDTO<RunEngineDTO>(runId);
        }

        public RunEngineDTO GetRunByPlayerAndTeamId(string playerId, string teamId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(path + "search/findByTeamIdAndPlayerId/?teamId=" + teamId + "&playerId=" + playerId); 

                    if(response != "")
                    {
                        return JsonConvert.DeserializeObject<RunEngineDTO>(response,
                                                    new JsonSerializerSettings
                                                    {
                                                        NullValueHandling = NullValueHandling.Ignore
                                                    });
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public GetAllDTO GetAllRunScore(string teamId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "allRunScore" + "?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex + "&metricId=" + metricId);

                    return JsonConvert.DeserializeObject<GetAllDTO>(responce,
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

        public long GetCountByTeamIdAndPlayerParentIsNotNull(string teamId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";

                    string responce = "";
                    responce = client.DownloadString(path + "search/countByTeamIdAndPlayerParentIsNotNull?teamId=" + teamId);

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


        public GetAllDTO GetAllRunScoreByEpisodeId(string episodeId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "runnersScoreByEpisode" + "?episodeId=" + episodeId + "&size=" + pageSize + "&page=" + pageIndex);

                    return JsonConvert.DeserializeObject<GetAllDTO>(responce,
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