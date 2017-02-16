using System;
using System.Linq;
using Vlast.Util.Data;

namespace Vlast.Broker.SNS.Model
{
    public class DeviceRepository
    {
        #region Singleton instance

        protected static object _syncRoot = new Object();
        private static volatile DeviceRepository instance;

        private DeviceRepository() { }

        public static DeviceRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (instance == null)
                            instance = new DeviceRepository();
                    }
                }
                return instance;
            }
        }



        /// <summary>
        /// Recupera o device de um usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DeviceEntity GetUserDevice(long userId, DeviceType deviceType)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                var query = from d in context.Devices
                            where d.UserId == userId && d.Status == GenericStatus.ACTIVE && d.DeviceType == deviceType
                            select d;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Recupera o primeiro device ativo do usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DeviceEntity GetUserDevice(long userId)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                var query = from d in context.Devices
                            where d.UserId == userId && d.Status == GenericStatus.ACTIVE 
                            select d;

                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// Remove todos os devices de um usuário
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool RemoveAllDevices(long userId)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                string query = "UPDATE  Push_Device SET Status = 0 WHERE  UserId = " + userId;
                int count = context.Database.ExecuteSqlCommand(query);
                return count > 0;
            }
        }

        /// <summary>
        /// Cria um novo device
        /// </summary>
        /// <param name="newDevice"></param>
        /// <returns></returns>
        public DeviceEntity CreateDevice(DeviceEntity newDevice)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                newDevice.Status = GenericStatus.ACTIVE;
                newDevice.LastUpdate = DateTime.UtcNow;
                context.Devices.Attach(newDevice);
                context.Entry(newDevice).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
            return newDevice;
        }

        /// <summary>
        /// Cria um novo device
        /// </summary>
        /// <param name="newDevice"></param>
        /// <returns></returns>
        public DeviceEntity UpdateDevice(DeviceEntity updateDevice)
        {
            using (BrokerModelContext context = new BrokerModelContext())
            {
                
                updateDevice.LastUpdate = DateTime.UtcNow;
                context.Devices.Attach(updateDevice);
                context.Entry(updateDevice).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            return updateDevice;
        }


        

        #endregion
    }
}
