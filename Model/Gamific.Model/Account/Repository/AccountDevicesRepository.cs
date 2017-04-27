using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Account.DTO;
using Vlast.Util.Data;

namespace Vlast.Gamific.Model.Account.Repository
{
    public class AccountDevicesRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile AccountDevicesRepository instance;

        private AccountDevicesRepository() { }

        public static AccountDevicesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new AccountDevicesRepository();
                    }
                }

                return instance;
            }
        }

        #endregion


        /// <summary>
        /// Busca por playerId
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public List<AccountDevicesEntity> FindByPlayerId(string playerId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from devices in context.AccountDevices
                            where devices.External_User_Id == playerId
                            select devices;

               
                return query.ToList();
            }
        }

        /// <summary>
        /// Busca por playerId
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public List<AccountDevicesDTO> FindAllByGameId(string gameId)
        {
            using (ModelContext context = new ModelContext())
            {
                var query = from devices in context.AccountDevices
                            from worker in context.Workers
                            from profile in context.Profiles
                            where worker.Status == GenericStatus.ACTIVE 
                            && worker.ExternalId == devices.External_User_Id
                            && worker.UserId == profile.Id
                            && worker.ExternalFirmId == gameId
                            select new AccountDevicesDTO()
                            {
                                External_User_Id = devices.External_User_Id,
                                Id = devices.Id,
                                Last_Update = devices.Last_Update,
                                Notification_Token = devices.Notification_Token,
                                Plataform = devices.Plataform,
                                UUID = devices.UUID,
                                PlayerName = profile.Name
                            };

                return query.ToList();
            }
        }
    }
}
