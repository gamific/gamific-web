using Gamific.Batch.Campaign;
using Gamific.Scheduler;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Vlast.Gamific.Web.Jobs;
using Vlast.Gamific.Web.Controllers.Util;
using Vlast.Gamific.Web.Services.Push;

namespace Vlast.Gamific.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        static RankingMailThread rankingJob = new RankingMailThread();

        protected void Application_Start()
        {
            

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string isMinify = Util.Parameter.ParameterCache.Get("MINIFY_COMPONENTES");

            if (true)
            {
                BundleTable.EnableOptimizations = true;
            }

            //rankingJob.Init(new TimeSpan(9, 36, 0));

            //ScriptsMigration.MigrationEmailToEngine();

            /*
            BannerEntity entity = BannerRepository.Instance.GetBannerById(3);
            entity.Image = File.ReadAllBytes(@"C:\Temp\tulio.jpg");
            BannerRepository.Instance.UpdateBanner(entity);
            */

        }
    }
}
