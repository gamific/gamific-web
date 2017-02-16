using System;
using System.Collections.Generic;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Gamific.Model.Firm.Repository;

namespace Gamific.Batch.Campaign
{
    public class CampaignOrchestration
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile CampaignOrchestration instance;

        private CampaignOrchestration() { }

        public static CampaignOrchestration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new CampaignOrchestration();
                    }
                }
                return instance;
            }
        }

        #endregion

        public List<CampaignEntity> GetCampaignsToInactive()
        {
            List<CampaignEntity> rtn = new List<CampaignEntity>();

            //rtn = CampaignRepository.Instance.GetAllToInactive();

            return rtn;
        }

    }
}
