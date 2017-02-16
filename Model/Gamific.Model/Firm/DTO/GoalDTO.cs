using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Gamific.Model.Firm.DTO
{
    public class GoalDTO
    {
        public int GoalId { get; set; }

        public long WorkerId { get; set; }

        public string ExternalPlayerId { get; set; }

        public string EpisodeId { get; set; }

        public long MetricId { get; set; }

        public string ExternalMetricId { get; set; }

        public string RunId { get; set; }

        public int Goal { get; set; }

        public string MetricName { get; set; }

        public string Icon { get; set; }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            GoalDTO other = obj as GoalDTO;
            if ((System.Object)other == null)
            {
                return false;
            }

            return other.GoalId == this.GoalId || other.MetricId == this.MetricId;
        }
    }
}
