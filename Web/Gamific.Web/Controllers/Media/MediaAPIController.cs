using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vlast.Gamific.Web.Controllers.Media
{
    [RoutePrefix("apiMedia")]
    public class MediaAPIController : BaseController
    {
        [Route("imagePath")]
        [HttpGet]
        public string GetImagePath()
        {
            return CurrentURL;
        }
    }
}