
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vlast.Util.Data
{
    public class DBHelper
    {
        /// <summary>
        /// Executa uma consulta customizada e retorna a lista de objetos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<T> ExecuteQuery<T>(DbContext context, string query) where T : new()
        {
            MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query, (MySqlConnection)context.Database.Connection);

          
          

            /*NpgsqlCommand command = new NpgsqlCommand(query, (NpgsqlConnection)context.Database.Connection);
             NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);

             DataTable resultData = new DataTable(typeof(T).Name);
             dataAdapter.Fill(resultData);

             return ToList<T>(resultData);*/
            return null;
        }


        /// <summary>
        /// Executa operações de update e delete diretamente na base de dados
        /// </summary>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbContext context, string query)
        {
            MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query, (MySqlConnection)context.Database.Connection);

            return command.ExecuteNonQuery();
        }


        /// <summary>
        /// Converts datatable to list<T> dynamically
        /// </summary>
        /// <typeparam name="T">Class name</typeparam>
        /// <param name="dataTable">data table to convert</param>
        /// <returns>List<T></returns>
        private static List<T> ToList<T>(DataTable dataTable) where T : new()
        {
            var dataList = new List<T>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            //Read Attribute Names and Types
            var objFieldNames = typeof(T).GetProperties(flags).Cast<PropertyInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType,
                    Info = item
                }).ToList();

            //Read Datatable column names and types
            var dtlFieldNames = dataTable.Columns.Cast<DataColumn>().
                Select(item => new
                {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var classObj = new T();

                foreach (var dtField in dtlFieldNames)
                {
                    PropertyInfo propertyInfo = null;

                    var fieldMetadata = objFieldNames.FirstOrDefault(p => p.Name.Equals(dtField.Name));

                    propertyInfo = fieldMetadata != null ? fieldMetadata.Info : null;

                    var field = objFieldNames.Find(x => x.Name == dtField.Name);

                    object value = dataRow[dtField.Name];

                    if (field != null && propertyInfo != null)
                    {
                        if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            propertyInfo.SetValue(classObj, ConvertToDateTime(value), null);
                        }
                        else if (propertyInfo.PropertyType == typeof(int))
                        {
                            propertyInfo.SetValue(classObj, ConvertToInt(value), null);
                        }
                        else if (propertyInfo.PropertyType == typeof(long))
                        {
                            propertyInfo.SetValue(classObj, ConvertToLong(value), null);
                        }
                        else if (propertyInfo.PropertyType == typeof(decimal))
                        {
                            propertyInfo.SetValue(classObj, ConvertToDecimal(value), null);
                        }
                        else if (propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(classObj, Enum.Parse(propertyInfo.PropertyType, ConvertToString(value)), null);
                        }
                        else if (propertyInfo.PropertyType == typeof(String))
                        {
                            if (dataRow[dtField.Name].GetType() == typeof(DateTime))
                            {
                                propertyInfo.SetValue(classObj, ConvertToDateString(value), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(classObj, ConvertToString(value), null);
                            }
                        }
                    }
                }
                dataList.Add(classObj);
            }
            return dataList;
        }

        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return Convert.ToDateTime(date).ToShortDateString();
        }

        private static string ConvertToString(object value)
        {
            if (value == null)
                return string.Empty;

            return Convert.ToString(value);
        }

        private static int ConvertToInt(object value)
        {
            if (value == null)
                return 0;

            return Convert.ToInt32(value);
        }

        private static long ConvertToLong(object value)
        {
            if (value == null)
                return 0;
            return Convert.ToInt64(value);
        }

        private static decimal ConvertToDecimal(object value)
        {
            if (value == null)
                return 0;

            return Convert.ToDecimal(value);
        }

        private static DateTime ConvertToDateTime(object date)
        {
            if (date == null)
                return DateTime.MinValue;

            return Convert.ToDateTime(date);
        }

        /// <summary>
        /// PRevine Sql Injections
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string SanitizeSqlParameter(string stringValue)
        {
            if (null == stringValue)
                return stringValue;

            stringValue = Regex.Replace(stringValue, "-{2,}", "-"); // transforms multiple --- in - use to comment in sql scripts
            stringValue = Regex.Replace(stringValue, @"[*/]+", string.Empty);// removes / and * used also to comment in sql scripts
            stringValue = Regex.Replace(stringValue, @"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);

            return stringValue;
        }
    }
}
