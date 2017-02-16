using System;
using System.ComponentModel.DataAnnotations;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    /// <summary>
    /// Classe com informações para buscar resultados
    /// </summary>
    public class FilterResultDTO
    {
        [Display(Name = "Data inicial")]
        public string InitialDate { get; set; }

        [Display(Name = "Data final")]
        public string EndDate { get; set; }

        public int TeamId { get; set; }

        public int PlayerId { get; set; }
    }
}