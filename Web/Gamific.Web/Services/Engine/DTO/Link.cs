using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class Link
    {
        public Self self { get; set; }
        public Player player { get; set; }

        public class Self
        {
            public string href { get; set; }
        }

        public class Player
        {
            public string href { get; set; }
        }
    }
}