using System.Runtime.Serialization;
using Vlast.Gamific.Account;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe com informações para edição de uma associação entre metrica e tipo de funcionario.
    /// </summary>
    public class WorkerTypeMetricDTO
    {
        public int Id { get; set; }

        public long WorkerTypeId { get; set; }

        public long MetricId { get; set; }

        public string MetricExternalId { get; set; }

        public string MetricName { get; set; }

        public string WorkerTypeName { get; set; }

        public string Icon { get; set; }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            WorkerTypeMetricDTO p = obj as WorkerTypeMetricDTO;
            if ((System.Object)p == null)
            {
                return false;
            }

            return p.Id == this.Id;
        }

    }
}
