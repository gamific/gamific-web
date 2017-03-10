using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class RunMetricEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile RunMetricEngineService instance;

        private RunMetricEngineService() : base(ENGINE_API + "runmetric/") { }

        public static RunMetricEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new RunMetricEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public RunMetricEngineDTO GetById(string episodeId)
        {
            return GetDTO<RunMetricEngineDTO>(episodeId);
        }

        public void AddRunsMetric(List<RunEngineDTO> runs, string metricId)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string json = JsonSerialize<List<RunEngineDTO>>(ref runs);
                    string response = client.UploadString(ENGINE_API + "addRunsMetric?" + "metricId=" + metricId, json);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateOrUpdate(RunMetricEngineDTO runMetric)
        {
            PostDTO<RunMetricEngineDTO>(ref runMetric);
        }

        public GetAllDTO findByRunIdAndMetricId(string runId, string metricId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(path + "search/findByRunIdAndMetricId?metricId=" + metricId + "&runId=" + runId + "&page=" + pageIndex + "&size=" + pageSize);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        #endregion
    }
}