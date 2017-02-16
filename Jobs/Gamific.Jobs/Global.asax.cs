using Gamific.Batch.Campaign;
using Gamific.Scheduler;
using System.Web;
using System.Web.Http;

namespace Gamific.Jobs
{
    public class WebApiApplication : HttpApplication
    {
        static WorkerFactory<CampaignThread> campaignJob = new WorkerFactory<CampaignThread>();

        protected void Application_Start()
        {

            //GlobalConfiguration.Configure(WebApiConfig.Register);

            campaignJob.Init(1);
            campaignJob.Start();
        }
    }
}
