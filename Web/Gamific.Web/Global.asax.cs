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
using System.Threading;
using System.Globalization;
using Vlast.Gamific.Model.Firm.DTO;
using Vlast.Gamific.Web.Services.Account.BIZ;
using Vlast.Gamific.Model.Firm.Repository;
using Vlast.Gamific.Model.Firm.Domain;
using System.Collections.Generic;

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

            InternalReportThread e = new InternalReportThread();

            string isMinify = Util.Parameter.ParameterCache.Get("MINIFY_COMPONENTES");

            if (true)
            {
                BundleTable.EnableOptimizations = true;
            }

            // rankingJob.Init(new TimeSpan(13, 00, 0));
            // e.Init(new TimeSpan(8, 0, 0));

            //ScriptsMigration.MigrationEmailToEngine();

            /*
            BannerEntity entity = BannerRepository.Instance.GetBannerById(3);
            entity.Image = File.ReadAllBytes(@"C:\Temp\tulio.jpg");
            BannerRepository.Instance.UpdateBanner(entity);
            */
        }
    }
}
