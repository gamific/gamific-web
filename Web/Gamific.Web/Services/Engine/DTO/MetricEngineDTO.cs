using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    [DataContract]
    public class MetricEngineDTO
    {

 
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [JsonProperty("name")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "A firma é obrigatório.")]
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "O limite inferior é obrigatório.")]
        [JsonProperty("floor")]
        public int? Floor { get; set; }

        //[Required(ErrorMessage = "O limite superior é obrigatório.")]
        [JsonProperty("ceiling")]
        public int? Ceiling { get; set; }

        [Required(ErrorMessage = "O peso é obrigatório.")]
        [JsonProperty("multiplier")]
        public int? Multiplier { get; set; }

        //[Required(ErrorMessage = "O xp é obrigatório.")]
        [JsonProperty("xp")]
        public int? Xp { get; set; }

        [JsonProperty("media")]
        public bool IsAverage { get; set; }

        [JsonProperty("inverse")]
        public bool IsInverse { get; set; }

        [Required(ErrorMessage = "O ícone é obrigatório.")]
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}