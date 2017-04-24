using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class TeamEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile TeamEngineService instance;

        private TeamEngineService() : base(ENGINE_API + "team/") { }

        public static TeamEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new TeamEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public TeamEngineDTO CreateOrUpdate(TeamEngineDTO team)
        {
            return PostDTO<TeamEngineDTO>(ref team);
        }

        public TeamEngineDTO GetById(string teamId)
        {
            return GetDTO<TeamEngineDTO>(teamId);
        }

        public TeamEngineDTO GetById(string teamId, string email)
        {
            return GetDTO<TeamEngineDTO>(teamId, email);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public List<RunEngineDTO> JoinPlayersOnTeam(string teamId, List<PlayerEngineDTO> playersToJoin)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string json = JsonSerialize<List<PlayerEngineDTO>>(ref playersToJoin);
                    string response = client.UploadString(ENGINE_API + "joinPlayersOnTeam?teamId=" + teamId, "POST", json);
                    return JsonDeserialize<List<RunEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemovePlayerOnTeam(string playerId, string teamId, int deleteScore = 0)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.UploadString(ENGINE_API + "removePlayerOnTeam?playerId=" + playerId + "&teamId=" + teamId + "&deleteScore=" + deleteScore, "DELETE", "");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO FindByEpisodeId(string episodeId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByEpisodeId" + "?episodeId=" + episodeId);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO FindByEpisodeId(string episodeId, string email)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;

                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                string response = client.DownloadString(path + "search/findByEpisodeId" + "?episodeId=" + episodeId);
                return JsonDeserialize<GetAllDTO>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO FindByEpisodeIdAndGameId(string episodeId, string gameId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByGameIdAndEpisodeId?episodeId=" + episodeId + "&gameId=" + gameId + "&page=" + pageIndex + "&size=" + pageSize);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO resultsByTeamIdAndMetricId(string teamId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "resultsByTeamIdAndMetricId" + "?teamId=" + teamId + "&metricId=" + metricId + "&page=" + pageIndex + "&size=" + pageSize);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetAllTeamScoreByEpisodeId(string episodeId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string responce = client.DownloadString(ENGINE_API + "allTeamScoreByEpisodeId" + "?episodeId=" + episodeId + "&size=" + pageSize + "&page=" + pageIndex + "&metricId=" + metricId);
                    return JsonDeserialize<GetAllDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetAllTeamScoreByEpisodeId(string episodeId, string metricId, string email, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;

                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;

                string responce = client.DownloadString(ENGINE_API + "allTeamScoreByEpisodeId" + "?episodeId=" + episodeId + "&size=" + pageSize + "&page=" + pageIndex + "&metricId=" + metricId);
                return JsonDeserialize<GetAllDTO>(responce);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TeamEngineDTO> GetAllTeamsByPlayerId(string gameId, string playerId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string responce = client.DownloadString(ENGINE_API + "teamsByPlayerId?playerId=" + playerId + "&gameId=" + gameId);
                    return JsonDeserialize<List<TeamEngineDTO>>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TeamEngineDTO> GetAllTeamsByGameId(string gameId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string responce = client.DownloadString(ENGINE_API + "teamsByGameId?gameId=" + gameId);
                    return JsonDeserialize<List<TeamEngineDTO>>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TeamEngineDTO UpdateTeamMaster(string masterPlayerId, string teamId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string responce = client.UploadString(ENGINE_API + "updateTeamMaster?masterPlayerId=" + masterPlayerId + "&teamId=" + teamId, "POST","");
                    return JsonDeserialize<TeamEngineDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TeamEngineDTO RemoveTeamFromEpisode(string teamId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string responce = client.UploadString(ENGINE_API + "removeTeamFromEpisode?teamId=" + teamId, "DELETE", "");
                    return JsonDeserialize<TeamEngineDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TeamEngineDTO GetByEpisodeIdAndNick(string episodeId, string nick)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string responce = client.DownloadString(ENGINE_API + "team/search/findByEpisodeIdAndNick?episodeId=" + episodeId + "&nick=" + nick);
                    return JsonDeserialize<TeamEngineDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TeamEngineDTO> TeamsByPlayerIdAndEpisodeId(string episodeId, string playerId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string responce = client.DownloadString(ENGINE_API + "teamsByPlayerIdAndEpisodeId?episodeId=" + episodeId + "&playerId=" + playerId);
                    return JsonDeserialize<List<TeamEngineDTO>>(responce);
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