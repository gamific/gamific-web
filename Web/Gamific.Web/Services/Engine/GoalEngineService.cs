using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Vlast.Gamific.Web.Services.Engine.DTO;

namespace Vlast.Gamific.Web.Services.Engine
{
    public class GoalEngineService : EngineServiceBase
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile GoalEngineService instance;

        private GoalEngineService() : base(ENGINE_API + "goal/") { }

        public static GoalEngineService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new GoalEngineService();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Services

        public GoalEngineDTO CreateOrUpdate(GoalEngineDTO goal)
        {
            return PostDTO<GoalEngineDTO>(ref goal);
        }

        public GoalEngineDTO GetById(string goalId)
        {
            return GetDTO<GoalEngineDTO>(goalId);
        }

        public void DeleteById(string id)
        {
            Delete(id);
        }

        public GoalEngineDTO GetByGameId(string gameId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "/search/findByGameId?gameId=" + gameId);

                    return JsonConvert.DeserializeObject<GoalEngineDTO>(responce);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public GoalEngineDTO GetByRunIdAndMetricId(string runId, string metricId)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    client.Encoding = System.Text.Encoding.UTF8;

                    string responce = "";
                    responce = client.DownloadString(path + "/search/findByMetricIdAndRunId?runId=" + runId + "&metricId=" + metricId);

                    return JsonConvert.DeserializeObject<GoalEngineDTO>(responce);
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

/*
        [Route("createGoal")]
        [HttpPost]
        [AllowAnonymous]
        public string CreateGoal(GoalEngineDTO goalDTO)
        {
            goalDTO = GoalEngineService.Instance.CreateOrUpdate(goalDTO);

            return JsonConvert.SerializeObject(
                    goalDTO,
                    Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                  );
        }

        [Route("getGoalById")]
        [HttpGet]
        [AllowAnonymous]
        public string GetGoalById(string id)
        {
            GoalEngineDTO goal = GoalEngineService.Instance.GetById(id);

            return JsonConvert.SerializeObject(
                    goal,
                    Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                  );
        }

        [Route("deleteGoalById")]
        [HttpPost]
        [AllowAnonymous]
        public string DeleteGoalById(string id)
        {
            GoalEngineService.Instance.DeleteById(id);

            return "ok";
        } 
     
*/
