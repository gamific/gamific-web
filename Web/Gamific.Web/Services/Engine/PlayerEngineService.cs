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

        public PlayerEngineDTO CreateOrUpdate(PlayerEngineDTO player, string email)
        {
            return PostDTO<PlayerEngineDTO>(ref player, email);
        }

        public PlayerEngineDTO GetById(string playerId)
        {
            return GetDTO<PlayerEngineDTO>(playerId);
        }

        public PlayerEngineDTO GetById(string playerId, string email)
        {
            return GetDTO<PlayerEngineDTO>(playerId, email);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public PlayerEngineDTO GetByGameIdAndNick(string gameId, string nick)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "/search/findByGameIdAndNick?gameId=" + gameId + "&nick=" + nick);
                    return JsonDeserialize<PlayerEngineDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO GetByGameIdAndActiveIsTrue(string gameId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "/search/findByGameIdAndActiveIsTrue?gameId=" + gameId);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PlayerEngineDTO GetByEmail(string email)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;
                    string response = client.DownloadString(path + "search/findByEmail?email=" + email);
                    return JsonDeserialize<PlayerEngineDTO>(response);
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