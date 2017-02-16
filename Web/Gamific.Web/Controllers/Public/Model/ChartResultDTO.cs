using System.Collections.Generic;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    /// <summary>
    /// Classe com informações para buscar resultados dos graficos
    /// </summary>
    public class ChartResultDTO
    {
        public List<List<int>> Positions { get; set; }

        public string MetricName { get; set; }
    }
}