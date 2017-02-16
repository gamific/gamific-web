using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Web.Services.Engine.BIZ
{
    public class EngineBIZ
    {
        #region API's URLs

        private static string ENGINE_API = "http://ec2-54-87-167-123.compute-1.amazonaws.com:8080/";

        #endregion

        #region API

        public static List<EngineTeamDTO> GetTeamsByEpisode(string episodeId)
        {
            List<EngineTeamDTO> rtn = new List<EngineTeamDTO>();

            try
            {
                WebClient client = new WebClient();

                client.Headers["User-Agent"] = "Mozilla / 5.0(Linux; Android 6.0.1; MotoG3 Build/ MPI24.107 - 55) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.81 Mobile Safari/ 537.36";

                string data = client.DownloadString(ENGINE_API + "team/search/findByEpisodeId?episodeId=" + episodeId);

                JObject currencyJson = JObject.Parse(data);

                var query = from c in currencyJson["_embedded"]["team"]
                            select new EngineTeamDTO()
                            {
                                Name = c["nick"].Value<string>(),
                                IdExterno = c["id"].Value<string>(),
                                Score = (int?)c["score"]
                            };

                rtn = query.ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return rtn;
        }

        public static List<EpisodeEngineDTO> GetEpisodesByGame(string gameId)
        {
            List<EpisodeEngineDTO> rtn = new List<EpisodeEngineDTO>();

            try
            {
                WebClient client = new WebClient();

                client.Headers["User-Agent"] = "Mozilla / 5.0(Linux; Android 6.0.1; MotoG3 Build/ MPI24.107 - 55) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.81 Mobile Safari/ 537.36";

                string data = client.DownloadString(ENGINE_API + "episode/search/findByGameId?gameId=" + gameId);

                JObject currencyJson = JObject.Parse(data);

                var query = from c in currencyJson["_embedded"]["episode"]
                            select new EpisodeEngineDTO()
                            {
                                Name = c["name"].Value<string>(),
                                Id = c["id"].Value<string>(),
                            };

                rtn = query.ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return rtn;
        }

        public static RankingDTO GetTeamScoreByTeamId(string teamId)
        {
            RankingDTO rtn = new RankingDTO();

            try
            {
                WebClient client = new WebClient();

                client.Headers["User-Agent"] = "Mozilla / 5.0(Linux; Android 6.0.1; MotoG3 Build/ MPI24.107 - 55) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.81 Mobile Safari/ 537.36";

                string data = client.DownloadString(ENGINE_API + "teamScore?teamId=" + teamId);

                JObject currencyJson = JObject.Parse(data);

                rtn = new RankingDTO()
                {
                    PlayerId = currencyJson.Root["id"].Value<string>(),
                    PlayerName = currencyJson.Root["nick"].Value<string>(),
                    Score = (int?)currencyJson.Root["score"]
                };

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return rtn;
        }

        public static RankingDTO GetGlobalScoreInEpisodeByEpisodeAndPlayer(string episodeId, string playerId)
        {
            RankingDTO dto = new RankingDTO();

            try
            {
                WebClient client = new WebClient();

                client.Headers["User-Agent"] = "Mozilla / 5.0(Linux; Android 6.0.1; MotoG3 Build/ MPI24.107 - 55) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.81 Mobile Safari/ 537.36";

                string data = client.DownloadString(ENGINE_API + "playerGlobalEpisodeScore?episodeId=" + episodeId + "&playerId=" + playerId);

                dto.Score = int.Parse(data);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return dto;
        }

        public static RankingDTO GetScoreByRun(string runId)
        {
            RankingDTO dto = new RankingDTO();

            try
            {
                WebClient client = new WebClient();

                client.Headers["User-Agent"] = "Mozilla / 5.0(Linux; Android 6.0.1; MotoG3 Build/ MPI24.107 - 55) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.81 Mobile Safari/ 537.36";

                string data = client.DownloadString(ENGINE_API + "runXp?runId=" + runId);

                JObject currencyJson = JObject.Parse(data);

                dto = new RankingDTO()
                {
                    PlayerId = currencyJson.Root["playerId"].Value<string>(),
                    Score = (int?)currencyJson.Root["score"]
                };

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return dto;
        }



        public static string getHierarchy(string episodeId)
        {
            RankingDTO dto = new RankingDTO();

            try
            {
                WebClient client = new WebClient();

                client.Headers["User-Agent"] = "Mozilla / 5.0(Linux; Android 6.0.1; MotoG3 Build/ MPI24.107 - 55) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.81 Mobile Safari/ 537.36";

                string data = client.DownloadString(ENGINE_API + "getHierarchical?episodeId=" + episodeId );

                return data;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return "";
        }


        #endregion

    }

}