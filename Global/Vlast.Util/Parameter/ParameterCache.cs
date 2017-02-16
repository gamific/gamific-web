using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Util.Parameter
{
    /// <summary>
    /// Gerenciamento de parâmetros do sistema
    /// </summary>
    public class ParameterCache
    {
        protected static object _syncRoot = new Object();
        private static volatile Dictionary<string, ParameterEntity> parameters_cache;
        private static volatile Dictionary<string, ParameterEntity> environment_parameters_cache;
        private static String AWS_ACCESS_KEY_ID = "AWS_ACCESS_KEY_ID";
        private static String AWS_SECRET_KEY = "AWS_SECRET_KEY";
        private static String CONNECTION_STRING = "CONNECTION_STRING";
        private static String S3_BUCKET_NAME = "S3_BUCKET_NAME";
        public static String CB_SERVER = "CB_SERVER";
        public static String CB_USER_NAME = "CB_USER_NAME";
        public static String CB_USER_PASSWORD = "CB_USER_PASSWORD";

        /// <summary>
        /// Recupera o bucket parametrizado para aplicação
        /// </summary>
        public static string S3BUCKET
        {
            get
            {
                return Get(S3_BUCKET_NAME);
            }
        }

        public static string DB_CONNECTION_STRING
        {
            get
            {
                return Get(CONNECTION_STRING);
            }
        }

        /// <summary>
        ///  Load parameter cache from storage
        /// </summary>
        private static void Load()
        {
            lock (_syncRoot)
            {
                LoadEnvironmentParameters();
                LoadParameters();
            }
        }


        /// <summary>
        /// Loads enviroment access parameters
        /// </summary>
        private static void LoadEnvironmentParameters()
        {
            if (environment_parameters_cache == null)
            {
                environment_parameters_cache = new Dictionary<string, ParameterEntity>();

                foreach (string key in ConfigurationManager.AppSettings.Keys)
                {
                    environment_parameters_cache.Add(key, new ParameterEntity(key, ConfigurationManager.AppSettings[key]));
                }

            }
        }

        /// <summary>
        /// Loads all system parameters
        /// </summary>
        private static void LoadParameters()
        {
            parameters_cache = new Dictionary<string, ParameterEntity>();

        }

        /// <summary>
        /// Retorna o parametro se existente
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static string Get(string parameterName)
        {
            lock (_syncRoot)
            {
                if (environment_parameters_cache == null)
                {
                    Load();
                }

                if (environment_parameters_cache.ContainsKey(parameterName))
                {
                    return environment_parameters_cache[parameterName].Value;
                }
                else if (environment_parameters_cache.ContainsKey(parameterName))
                {
                    return parameters_cache[parameterName].Value;
                }

                return "";
            }

        }

    }
}
