using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web.Http;
using System.Web.Http.Cors;
using Vlast.Gamific.Model.Media.Repository;
using Vlast.Util.Instrumentation;

namespace Vlast.Gamific.Api.Media
{
    /// <summary>
    /// Serviço de midia
    /// </summary>
    [RoutePrefix("api/media")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImageAPIController : ApiController
    {

        /// <summary>
        /// Recupera uma logo
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [Route("{imageId:int}")]
        [HttpGet]
        public HttpResponseMessage GetLogo(int imageId)
        {
            HttpResponseMessage result = null;
            try
            {
                if (imageId > 0)
                {
                    Stream mediaStream = ImageRepository.Instance.GetLogo(imageId);
                    if (mediaStream != null)
                    {
                        result = ServiceHelper.CreateCachedResponse(Request, mediaStream, MediaTypeNames.Image.Jpeg, "logo-" + imageId + "-image");
                    }
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }

            catch (Exception ex)
            {
                ServiceHelper.ThrowError(ex);
            }

            return result;
        }

    }
}