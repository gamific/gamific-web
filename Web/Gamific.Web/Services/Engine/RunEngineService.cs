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

   
        public GetAllDTO GetRunsByTeamIdAuth(string teamId, string email, int pageIndex = 0, int pageSize = 10 )
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                string response = client.DownloadString(path + "search/findByTeamId/?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex);
                return JsonDeserialize<GetAllDTO>(response);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetRunsByTeamId(string teamId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByTeamId/?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetRunsByTeamId(string teamId, string email, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient(email))
                {
                    string response = client.DownloadString(path + "search/findByTeamId?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex);
                    return JsonDeserialize<GetAllDTO>(response);
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByTeamIdAndPlayerId/?teamId=" + teamId + "&playerId=" + playerId);
                    return JsonDeserialize<RunEngineDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RunEngineDTO GetRunByPlayerAndTeamId(string playerId, string teamId, string email)
        {
            try
            {
                using (WebClient client = GetClient(email))
                {
                    string response = client.DownloadString(path + "search/findByTeamIdAndPlayerId/?teamId=" + teamId + "&playerId=" + playerId);
                    return JsonDeserialize<RunEngineDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RunEngineDTO GetRunByPlayerAndTeamId(string playerId, string teamId, string email)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                string response = client.DownloadString(path + "search/findByTeamIdAndPlayerId/?teamId=" + teamId + "&playerId=" + playerId);
                    return JsonDeserialize<RunEngineDTO>(response);

                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RunEngineDTO GetRunByPlayerAndTeamIdAuth(string playerId, string teamId, string email)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                string response = client.DownloadString(path + "search/findByTeamIdAndPlayerId/?teamId=" + teamId + "&playerId=" + playerId);
                    return JsonDeserialize<RunEngineDTO>(response);
                
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

        public GetAllDTO GetAllRunScore(string teamId, string metricId,string email, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                
                    string response = client.DownloadString(ENGINE_API + "allRunScore" + "?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex + "&metricId=" + metricId);
                    return JsonDeserialize<GetAllDTO>(response);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetAllRunScore(string teamId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "allRunScore" + "?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex + "&metricId=" + metricId);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetAllRunScore(string teamId, string metricId, string email, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;

                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;

                string response = client.DownloadString(ENGINE_API + "allRunScore" + "?teamId=" + teamId + "&size=" + pageSize + "&page=" + pageIndex + "&metricId=" + metricId);
                return JsonDeserialize<GetAllDTO>(response);
                
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/countByTeamIdAndPlayerParentIsNotNull?teamId=" + teamId);
                    return JsonDeserialize<long>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public long GetCountByTeamIdAndPlayerParentIsNotNull(string teamId, string email)
        {
            try
            {
                using (WebClient client = GetClient(email))
                {
                    string response = client.DownloadString(path + "search/countByTeamIdAndPlayerParentIsNotNull?teamId=" + teamId);
                    return JsonDeserialize<long>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public long GetCountByTeamIdAndPlayerParentIsNotNullAuth(string teamId, string email)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;

                string response = client.DownloadString(path + "search/countByTeamIdAndPlayerParentIsNotNull?teamId=" + teamId);
                    return JsonDeserialize<long>(response);
                
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "runnersScoreByEpisode" + "?episodeId=" + episodeId + "&size=" + pageSize + "&page=" + pageIndex);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public GetAllDTO ScoreByEpisodeIdAndMetricId(string episodeId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "runnersScoreByEpisode?episodeId=" + episodeId + "&metricId=" + metricId + "&page=" + pageIndex + "&size=" + pageSize);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RunEngineDTO GetByEpisodeIdAndPlayerId(string episodeId, string playerId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "findRunByEpisodeId?episodeId=" + episodeId + "&playerId=" + playerId);
                    return JsonDeserialize<RunEngineDTO>(response);
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