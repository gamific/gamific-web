using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class HallOfFameEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile HallOfFameEngineService instance;

        private HallOfFameEngineService() : base(ENGINE_API + "hallOfFame/") { }

        public static HallOfFameEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new HallOfFameEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public HallOfFameEngineDTO CreateOrUpdate(HallOfFameEngineDTO hallOfFame)
        {
            return PostDTO<HallOfFameEngineDTO>(ref hallOfFame);
        }

        public HallOfFameEngineDTO GetById(string hallOfFameId)
        {
            return GetDTO<HallOfFameEngineDTO>(hallOfFameId);
        }

        public GetAllDTO GetByGameId(string gameId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByGameId?gameId=" + gameId);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetByEpisodeId(string episodeId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByEpisodeId?episodeId=" + episodeId);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteByEpisodeId(string episodeId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    client.DownloadString(ENGINE_API + "deleteHallOfFameByEpisodeId?episodeId=" + episodeId);

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