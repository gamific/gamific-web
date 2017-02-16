using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql.AspNet.Identity
{
    public class MySqlConnectionWrapper
    {
        internal static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["CONNECTION_STRING"];
            }
        }
    }
}
