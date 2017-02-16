using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Util.Data
{
    /// <summary>
    /// Utilizado para entidades que possuem status ativo e inativo
    /// </summary>
    public enum GenericStatus
    {
        [Display(Name ="Inativo")]
        INACTIVE,

        [Display(Name = "Ativo")]
        ACTIVE
    }

  
}
