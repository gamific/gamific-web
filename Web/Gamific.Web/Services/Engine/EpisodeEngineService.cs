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

        public EpisodeEngineDTO GetById(string episodeId, string email)
        {
            return GetDTO<EpisodeEngineDTO>(episodeId, email);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public void CloseById(string id)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.UploadString(ENGINE_API + "closeEpisode?episodeId=" + id, "POST");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetByGameIdAndActiveIsTrue(string gameId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByGameIdAndActiveIsTrue/" + "?gameId=" + gameId + "&page=" + pageIndex + "&size=" + pageSize);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public EpisodeEngineDTO Clone(string name, string id, long initDate, long finishDate)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.UploadString(ENGINE_API + "cloneEpisode?name=" + name + "&episodeId=" + id + "&initDate=" + initDate + "&finishDate=" + finishDate, "POST");
                    return JsonDeserialize<EpisodeEngineDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<EpisodeEngineDTO> Clean(string episodeId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "deleteAllScoreByEpisodeId?episodeId=" + episodeId); //, "GET");
                    return JsonDeserialize<List<EpisodeEngineDTO>>(response);
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByGameIdAndActive" + "?size=" + pageSize + "&page=" + pageIndex + "&gameId=" + gameId + "&active=" + isActive);
                    return JsonDeserialize<GetAllDTO>(response);
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByGameIdAndActive/" + "?gameId=" + gameId + "&active=" + active);
                    GetAllDTO all = JsonDeserialize<GetAllDTO>(response);
                    all.List.episode = all.List.episode.OrderBy(x => x.initDate).ToList();
                    return all;
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByGameId/" + "?gameId=" + gameId + "&page=" + pageIndex + "&size=" + pageSize);
                    GetAllDTO all = JsonDeserialize<GetAllDTO>(response);
                    all.List.episode = all.List.episode.OrderBy(x => x.initDate).ToList();
                    return all;
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "resultsByEpisodeIdAndMetricId" + "?episodeId=" + episodeId + "&metricId=" + metricId + "&page=" + pageIndex + "&size=" + pageSize);
                    return JsonDeserialize<GetAllDTO>(response);
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "episodeByPlayerId" + "?playerId=" + playerId + "&gameId=" + gameId + "&active=" + active);
                    return JsonDeserialize<List<EpisodeEngineDTO>>(response);
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "countPlayersByEpisodeId?episodeId=" + episodeId);
                    return JsonDeserialize<long>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteAllScoreByEpisodeId(string episodeId) /* List<EpisodeEngineDTO> */
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    /*string response = */
                    client.DownloadString(ENGINE_API + "deleteAllScoreByEpisodeId?episodeId=" + episodeId);
                    /*return JsonDeserialize<List<EpisodeEngineDTO>>(response);*/
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