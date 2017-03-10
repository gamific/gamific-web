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
    public class MetricEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile MetricEngineService instance;

        private MetricEngineService() : base(ENGINE_API + "metric/") { }

        public static MetricEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new MetricEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public MetricEngineDTO CreateOrUpdate(MetricEngineDTO metric)
        {
            return PostDTO<MetricEngineDTO>(ref metric);
        }

        public MetricEngineDTO GetById(string metricId)
        {
            return GetDTO<MetricEngineDTO>(metricId);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public GetAllDTO GetAllDTOByGame(string gameId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(path + "search/findByGameId?gameId=" + gameId + "&size=" + pageSize + "&page=" + pageIndex);
                    return JsonDeserialize<GetAllDTO>(response);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public MetricEngineDTO GetDTOByGameAndName(string gameId, string name)
        {
            try
            {
                using (WebClient client = GetClient)
                {
                    string response = client.DownloadString(path + "search/findByGameIdAndName?gameId=" + gameId + "&name=" + name);
                    return JsonDeserialize<MetricEngineDTO>(response);
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