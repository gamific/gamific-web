using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class ItemEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile ItemEngineService instance;

        private ItemEngineService() : base(ENGINE_API + "item") { }

        public static ItemEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new ItemEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public ItemEngineDTO CreateOrUpdate(ItemEngineDTO item)
        {
            return PostDTO<ItemEngineDTO>(ref item);
        }

        public ItemEngineDTO GetById(string itemId)
        {
            return GetDTO<ItemEngineDTO>(itemId);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public ItemEngineDTO FindByNameAndGameId(string name, string gameId)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(path + "/search/findByNameAndGameId?gameId=" + gameId + "&name=" + name);
                    return JsonDeserialize<ItemEngineDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemEngineDTO> FindByTeam(string metricId, string teamId)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(ENGINE_API + "/itensByTeamId?metricId=" + metricId + "&teamId=" + teamId);
                    return JsonDeserialize<List<ItemEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemEngineDTO> FindByEpisode(string metricId, string episodeId)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(ENGINE_API + "itensByEpisodeId?metricId=" + metricId + "&episodeId=" + episodeId);
                    return JsonDeserialize<List<ItemEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemEngineDTO> FindByRun(string metricId, string runId)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(ENGINE_API + "/itensByRunId?metricId=" + metricId + "&runId=" + runId);
                    return JsonDeserialize<List<ItemEngineDTO>>(response);
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