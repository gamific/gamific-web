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

        public void AddRunsMetric(List<RunEngineDTO> runsId, string metricId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string json = JsonConvert.SerializeObject(runsId);

                    string response = "";
                    response = client.UploadString(ENGINE_API + "addRunsMetric?" + "metricId=" + metricId, json);
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
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string response = "";
                    response = client.DownloadString(path + "search/findByRunIdAndMetricId?metricId=" + metricId + "&runId=" + runId + "&page=" + pageIndex + "&size=" + pageSize);

                    return JsonConvert.DeserializeObject<GetAllDTO>(response, 
                                                                    new JsonSerializerSettings{
                                                                                        NullValueHandling = NullValueHandling.Ignore
                                                                                    });
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