using System.Collections.Generic;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe para passar parametros para o 3d
    /// </summary>
    public class CarDTO
    {
        public List<WorkerCarDTO> cars { get; set; }

        public int CompanyLogo { get; set; }

        public Target3D Target { get; set; }

        public int PercentFromGoalReached { get; set; }

        public int TotalGoal { get; set; }

        public string TargetName { get; set; }

        public int TargetLogoId { get; set; }

        public string LogoPathOutdoor1 { get; set; }

        public string LogoPathOutdoor2 { get; set; }

        public string LogoPathOutdoor3 { get; set; }

        public string LogoPathOutdoor4 { get; set; }

        public string LogoPathBanner1 { get; set; }

        public string LogoPathBanner2 { get; set; }

    }
}
