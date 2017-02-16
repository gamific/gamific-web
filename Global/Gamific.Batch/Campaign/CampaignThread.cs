using Vlast.Util.Instrumentation;
using System;
using System.Threading;
using Gamific.Scheduler;
using System.Collections.Generic;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.Repository;

namespace Gamific.Batch.Campaign
{
    /// <summary>
    /// Verifica a validade das campanhas
    /// </summary>
    public class CampaignThread : ThreadWorker
    {
        /// <summary>
        /// Periodicamente verifica as campanhas vencidas para inativa-las
        /// </summary>
        /// <returns></returns>
        internal override bool Run()
        {
            bool result = false;
            List<CampaignEntity> rtn = new List<CampaignEntity>();
            try
            {
                lock (CampaignOrchestration.Instance)
                {
                    rtn.AddRange(CampaignOrchestration.Instance.GetCampaignsToInactive());
                }

                foreach (CampaignEntity obj in rtn)
                {
                    //CampaignRepository.Instance.InactiveCampaign(obj.Id);
                }

            }
            catch (ThreadAbortException tex)
            {
                Logger.LogException(tex);
                result = false;
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                result = false;
                Logger.LogException(ex);
            }

            Console.Write("OLA ");

            return result;
        }
    }
}
