using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Gamific.Model.Public.DTO
{
    public class ResultMetricDTO
    {
        public int ResultId { get; set; }

        public int GoalId { get; set; }

        public int MetricId { get; set; }

        public int WorkerId { get; set; }

        public int Goal { get; set; }

        public int Result { get; set; }

        public int Points { get; set; }

        public string MetricIcon { get; set; }

        public string MetricName { get; set; }

        public string WorkerName { get; set; }

        public DateTime Date { get; set; }

    }
}
