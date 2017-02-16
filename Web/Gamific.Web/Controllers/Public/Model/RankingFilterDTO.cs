using System;
using System.ComponentModel.DataAnnotations;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    /// <summary>
    /// Classe com informações para buscar resultados
    /// </summary>
    public class RankingFilterDTO
    {
        [Display(Name = "workerTypeId")]
        public int WorkerTypeId { get; set; }

        [Display(Name = "teamId")]
        public string TeamId { get; set; }

        [Display(Name = "episodeId")]
        public string EpisodeId { get; set; }

    }
}