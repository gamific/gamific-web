using System.Collections.Generic;

namespace Vlast.Gamific.Web.Controllers.Public.Model
{
    public class LineDTO
    {

        public LineDTO()
        {
            Points = new List<LinePointDTO>();
        }

        public string Period { get; set; }

        public long dateLong { get; set; }

        public List<LinePointDTO> Points { get; set; }

    }
}