using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class EpisodeQuizEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile EpisodeQuizEngineService instance;

        private EpisodeQuizEngineService() : base(ENGINE_API + "quizSheet/") { }

        public static EpisodeQuizEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new EpisodeQuizEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public EpisodeQuizEngineDTO CreateOrUpdate(EpisodeQuizEngineDTO quiz)
        {
            return PostDTO<EpisodeQuizEngineDTO>(ref quiz);
        }

        public EpisodeQuizEngineDTO GetById(string quizId)
        {
            return GetDTO<EpisodeQuizEngineDTO>(quizId);
        }

        public void DeleteById(string quizId)
        {
            Delete(quizId);
        }

        public List<EpisodeQuizEngineDTO> GetByEpisodeId(string episodeId)
        {
            try
            {
                using (WebClient client = GetClient())
                {
                    string response = client.DownloadString(path + "search/findByEpisodeId?episodeId=" + episodeId);
                    return JsonDeserialize<List<EpisodeQuizEngineDTO>>(response);
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