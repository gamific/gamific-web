using System;
using Vlast.Gamific.Web.Services.Engine.DTO;
using System.Net;
using Newtonsoft.Json;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class PlayerEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile PlayerEngineService instance;

        private PlayerEngineService() : base(ENGINE_API + "player/") { }

        public static PlayerEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new PlayerEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public PlayerEngineDTO CreateOrUpdate(PlayerEngineDTO player)
        {
            return PostDTO<PlayerEngineDTO>(ref player);
        }

        public PlayerEngineDTO GetById(string playerId)
        {
            return GetDTO<PlayerEngineDTO>(playerId);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }


        /*public GetAllDTO GetAllByGameId(string gameId, int pageIndex = 0, int pageSize = 10000)
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
        }*/

        public PlayerEngineDTO GetByGameIdAndNick(string gameId, string nick)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "/search/findByGameIdAndNick?gameId=" + gameId + "&nick=" + nick);

                    return JsonConvert.DeserializeObject<PlayerEngineDTO>(responce);
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