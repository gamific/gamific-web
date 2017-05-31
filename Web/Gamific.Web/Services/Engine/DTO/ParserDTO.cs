using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class ParserDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idString")]
        public String IdString { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("idPrincipal")]
        public int IdPrincipal { get; set; }

        [JsonProperty("isRight")]
        public bool IsRight { get; set; }

    }

    }