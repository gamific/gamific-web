using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vlast.Gamific.Model.Account.Domain;
using Vlast.Gamific.Model.Firm.Domain;
using System.Web;
using Vlast.Gamific.Web.Services.Engine;
using Vlast.Gamific.Web.Services.Engine.DTO;
using Vlast.Gamific.Model.Firm.DTO;

namespace Vlast.Gamific.Web.Controllers.Management.Model
{
    public class ImplantationDTO
    {
        public ImplantationDTO()
        {
            Episode = new EpisodeEngineDTO();
            WorkerType = new WorkerTypeEntity();
            Worker = new WorkerDTO();
            Team = new TeamEngineDTO();
            Metric = new MetricEngineDTO();
            //Goal = new IEnumerable<GoalEngineDTO>();

        }
        public EpisodeEngineDTO Episode { get; set; }

        public WorkerTypeEntity WorkerType { get; set; }

        public WorkerDTO Worker { get; set; }

        public TeamEngineDTO Team { get; set; }

        public MetricEngineDTO Metric { get; set; }

        public IEnumerable<GoalEngineDTO> Goal { get; set; }

    }
}
