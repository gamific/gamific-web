using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Util.Global
{
    public class TemplateHelper
    {

        static string REPEATING_START = "#START#";
        static string REPEATING_END = "#END#";

        /// <summary>
        /// Dado um template e um dicionário de dados fas as substituições e retorna o arquivo
        /// gerado do template com os dados
        /// </summary>
        /// <param name="template"></param>
        /// <param name="data"></param>
        public static string Format(string template, Dictionary<string, string> data)
        {
            if (!string.IsNullOrEmpty(template))
            {
                foreach (string key in data.Keys)
                {
                    string parm = "#{" + key + "}";
                    template = template.Replace(parm, data[key]);
                }

                return template;
            }
            return "";
        }

        /// <summary>
        /// Dado um template e um dicionário de dados fas as substituições e retorna o arquivo
        /// gerado do template com os dados
        /// </summary>
        /// <param name="template"></param>
        /// <param name="globalData"></param>
        /// <param name="rowsData"></param>
        public static string Format(string template, Dictionary<string, string> globalData, List<Dictionary<string, string>> rowsData)
        {
            string result = "";
            if (!string.IsNullOrEmpty(template))
            {
                result = Format(template, globalData);

                result = result.Replace(REPEATING_END, REPEATING_START);
                string[] parts = result.Split(new string[] { REPEATING_START }, StringSplitOptions.RemoveEmptyEntries);

                string header = parts[0];
                string footer = parts[2];
                string templatePart = parts[1];


                string htmlContent = "";
                foreach (var row in rowsData)
                {
                    htmlContent += Format(templatePart, row);
                }

                result = header + htmlContent + footer;
            }

            return result;
        }

    }
}
