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

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public List<RunEngineDTO> JoinPlayersOnTeam(string teamId, List<PlayerEngineDTO> playersToJoin)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string json = JsonConvert.SerializeObject(playersToJoin,
                                                                Formatting.None,
                                                                new JsonSerializerSettings
                                                                {
                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                });

                    string response = "";
                    response = client.UploadString(ENGINE_API + "joinPlayersOnTeam?teamId=" + teamId, "POST", json);

                    return JsonConvert.DeserializeObject<List<RunEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public void RemovePlayerOnTeam(string playerId, string teamId, int deleteScore = 0)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string teste = ENGINE_API + "removePlayerOnTeam?playerId=" + playerId + "&teamId=" + teamId + "&deleteScore=" + deleteScore;

                    string response = "";
                    response = client.UploadString(ENGINE_API + "removePlayerOnTeam?playerId=" + playerId + "&teamId=" + teamId + "&deleteScore=" + deleteScore, "DELETE", "");
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public GetAllDTO FindByEpisodeId(string episodeId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(path + "search/findByEpisodeId" + "?episodeId=" + episodeId);

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

        public GetAllDTO FindByEpisodeIdAndGameId(string episodeId, string gameId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(path + "search/findByGameIdAndEpisodeId?episodeId=" + episodeId + "&gameId=" + gameId + "&page=" + pageIndex + "&size=" + pageSize);

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

        public GetAllDTO resultsByTeamIdAndMetricId(string teamId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(ENGINE_API + "resultsByTeamIdAndMetricId" + "?teamId=" + teamId + "&metricId=" + metricId + "&page=" + pageIndex + "&size=" + pageSize);

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

        public GetAllDTO GetAllTeamScoreByEpisodeId(string episodeId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "allTeamScoreByEpisodeId" + "?episodeId=" + episodeId + "&size=" + pageSize + "&page=" + pageIndex + "&metricId=" + metricId);

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

        public List<TeamEngineDTO> GetAllTeamsByPlayerId(string gameId, string playerId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "teamsByPlayerId?playerId=" + playerId + "&gameId=" + gameId);

                    return JsonConvert.DeserializeObject<List<TeamEngineDTO>>(responce,
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

        public List<TeamEngineDTO> GetAllTeamsByGameId(string gameId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "teamsByGameId?gameId=" + gameId);

                    return JsonConvert.DeserializeObject<List<TeamEngineDTO>>(responce,
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

        public TeamEngineDTO UpdateTeamMaster(string masterPlayerId, string teamId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.UploadString(ENGINE_API + "updateTeamMaster?masterPlayerId=" + masterPlayerId + "&teamId=" + teamId, "POST","");

                    return JsonConvert.DeserializeObject<TeamEngineDTO>(responce,
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

        public TeamEngineDTO RemoveTeamFromEpisode(string teamId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.UploadString(ENGINE_API + "removeTeamFromEpisode?teamId=" + teamId, "DELETE", "");

                    return JsonConvert.DeserializeObject<TeamEngineDTO>(responce,
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

        public TeamEngineDTO GetByEpisodeIdAndNick(string episodeId, string nick)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "team/search/findByEpisodeIdAndNick?episodeId=" + episodeId + "&nick=" + nick);

                    return JsonConvert.DeserializeObject<TeamEngineDTO>(responce,
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

        public List<TeamEngineDTO> TeamsByPlayerIdAndEpisodeId(string episodeId, string playerId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(ENGINE_API + "teamsByPlayerIdAndEpisodeId?episodeId=" + episodeId + "&playerId=" + playerId);

                    return JsonConvert.DeserializeObject<List<TeamEngineDTO>>(responce,
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