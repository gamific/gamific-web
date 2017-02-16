using System;
using System.Collections.Generic;
using System.Linq;
using Vlast.Gamific.Model.Properties;
using Vlast.Gamific.Model.Firm.Domain;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Firm.Repository
{
    public class CampaignRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile CampaignRepository instance;

        private CampaignRepository() { }

        public static CampaignRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new CampaignRepository();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Campaing

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<CampaignEntity> GetAll()
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Campaigns
                        where sc.Status == GenericStatus.ACTIVE
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Query para consulta externa
        /// </summary>
        /// <returns></returns>
        public List<CampaignEntity> GetAllToInactive()
        {
            ModelContext context = new ModelContext();

            DateTime now = DateTime.Now.AddDays(-1);

            var query = from sc in context.Campaigns
                        where sc.Status == GenericStatus.ACTIVE && now >= sc.EndDate
                        orderby sc.Id ascending
                        select sc;

            return query.ToList();
        }

        /// <summary>
        /// Busca todas campanhas ativas de uma empresa
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<CampaignEntity> GetAllFromFirm(int firmId, int pageIndex = 0, int pageSize = 10)
        {
            ModelContext context = new ModelContext();
            var query = from sc in context.Campaigns
                        where sc.Status == GenericStatus.ACTIVE && sc.FirmId == firmId
                        orderby sc.Id ascending
                        select sc;

            return query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Recupera a campanha pelo id
        /// </summary>
        /// <param name="campaingId"></param>
        /// <returns></returns>
        public CampaignEntity GetById(int campaingId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query =
                            from t in context.Campaigns
                            where t.Status == GenericStatus.ACTIVE && t.Id == campaingId
                            select t;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Salva uma campanha na base de dados
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public CampaignEntity CreateCampaign(CampaignEntity newEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                newEntity.LastUpdate = DateTime.UtcNow;
                context.Campaigns.Attach(newEntity);
                context.Entry(newEntity).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newEntity;
        }

        /// <summary>
        /// Atualiza uma campanha
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public CampaignEntity UpdateCampaign(CampaignEntity updatedEntity)
        {
            using (ModelContext context = new ModelContext())
            {
                updatedEntity.LastUpdate = DateTime.UtcNow;
                context.Campaigns.Attach(updatedEntity);
                context.Entry(updatedEntity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return updatedEntity;
        }

        #endregion
    }
}