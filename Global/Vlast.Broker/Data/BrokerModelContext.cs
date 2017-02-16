using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using Vlast.Broker.SNS.Model;
using Vlast.Util.Parameter;

namespace Vlast.Broker.SNS
{
    public class BrokerModelContext : DbContext
    {
        #region Entities
 
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<PushSubscriptionEntity> PushSubscriptions { get; set; }


        #endregion

        internal static int DEFAULT_PAGE_SIZE = 100;

        public ObjectContext ObjectContextInstance
        {
            get
            {
                return (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
            }
        }
 
        public static CultureInfo DataBaseCulture = CultureInfo.CreateSpecificCulture("en-US");

        public BrokerModelContext()
            : base("name=DB_CONNECTION")
        {
            Database.SetInitializer<BrokerModelContext>(null);
            Database.Connection.ConnectionString = ParameterCache.DB_CONNECTION_STRING;

#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }
 
    }
}


public enum DATABASES
{

    PROCESSADORA,
    CONVENIO,
    NOTIFICACAO
}

