using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class CardEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile CardEngineService instance;

        private CardEngineService() : base(ENGINE_API) { }

        public static CardEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new CardEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public CardEngineDTO EpisodeAndMetric(string episodeId, string metricId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "episodeCardByMetricId?episodeId=" + episodeId + "&metricId=" + metricId);

                    return JsonConvert.DeserializeObject<CardEngineDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CardEngineDTO> Episode(string gameId, string episodeId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "episodeCards?gameId=" + gameId + "&episodeId=" + episodeId);

                    return JsonConvert.DeserializeObject<List<CardEngineDTO>>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CardEngineDTO> Team(string gameId, string teamId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "teamCards?gameId=" + gameId + "&teamId=" + teamId);

                    return JsonConvert.DeserializeObject<List<CardEngineDTO>>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CardEngineDTO> Player(string gameId, string teamId, string playerId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "playerCards?gameId=" + gameId + "&teamId=" + teamId + "&playerId=" + playerId);

                    return JsonConvert.DeserializeObject<List<CardEngineDTO>>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO IndividualResultsByPlayerId(string playerId, string episodeId ,string gameId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(ENGINE_API + "cardsByPlayerId" + "?playerId=" + playerId + "&episodeId=" + episodeId + "&gameId=" + gameId);

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

        #endregion
    }
}