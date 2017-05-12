﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Web.Controllers.Public.Model;

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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "episodeCardByMetricId?episodeId=" + episodeId + "&metricId=" + metricId);
                    return JsonDeserialize<CardEngineDTO>(response);
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "episodeCards?gameId=" + gameId + "&episodeId=" + episodeId);
                    return JsonDeserialize<List<CardEngineDTO>>(response);
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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "teamCards?gameId=" + gameId + "&teamId=" + teamId);
                    return JsonDeserialize<List<CardEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CardEngineDTO> Team(string gameId, string teamId, string email)
        {
            try
            {
                using (WebClient client = GetClient(email))
                {
                    string response = client.DownloadString(path + "teamCards?gameId=" + gameId + "&teamId=" + teamId);
                    return JsonDeserialize<List<CardEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CardEngineDTO> TeamAuth(string gameId, string teamId, string email)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;

                string response = client.DownloadString(path + "teamCards?gameId=" + gameId + "&teamId=" + teamId);
                return JsonDeserialize<List<CardEngineDTO>>(response);

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
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "playerCards?gameId=" + gameId + "&teamId=" + teamId + "&playerId=" + playerId);
                    return JsonDeserialize<List<CardEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CardEngineDTO> Player(string gameId, string teamId, string playerId, string email)
        {
            try
            {
                using (WebClient client = GetClient(email))
                {
                    string response = client.DownloadString(path + "playerCards?gameId=" + gameId + "&teamId=" + teamId + "&playerId=" + playerId);
                    return JsonDeserialize<List<CardEngineDTO>>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CardEngineDTO> PlayerAuth(string gameId, string teamId, string playerId, string email)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Encoding = System.Text.Encoding.UTF8;
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(email + ":" + ""));
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + encoded;

                string response = client.DownloadString(path + "playerCards?gameId=" + gameId + "&teamId=" + teamId + "&playerId=" + playerId);
                return JsonDeserialize<List<CardEngineDTO>>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GetAllDTO IndividualResultsByPlayerId(string playerId, string episodeId, string gameId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(ENGINE_API + "cardsByPlayerId" + "?playerId=" + playerId + "&episodeId=" + episodeId + "&gameId=" + gameId);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<BarDTO> EpisodesAndMetrics(List<EpisodeEngineDTO> episodes, List<MetricEngineDTO> metrics)
        {
            using (WebClient client = GetClient())
            {
                try
                {

                    BarParamDTO dto = new BarParamDTO();

                    dto.Episodes = episodes;
                    dto.Metrics = metrics;

                    string response = client.UploadString(ENGINE_API + "graphicbars", "POST", JsonSerialize(ref dto));

                    return JsonDeserialize<List<BarDTO>>(response);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public ChartResultDTO GameAndMetricAndPeriod(List<string> episodesParam, string gameId, List<string> metrics, long initDate, long finishDate)
        {
            using (WebClient client = GetClient())
            {
                try
                {

                    ChartParamDTO dto = new ChartParamDTO();

                    dto.FinishDate = finishDate;
                    dto.InitDate = initDate;
                    dto.Metrics = metrics;
                    dto.GameId = gameId;
                    dto.Episodes = episodesParam;

                    string response = client.UploadString(ENGINE_API + "graphicLineWebByDate", "POST", JsonSerialize(ref dto));

                    return JsonDeserialize<ChartResultDTO>(response);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion
    }
}