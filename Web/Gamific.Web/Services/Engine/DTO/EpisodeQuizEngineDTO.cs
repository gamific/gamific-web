using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    [DataContract]
    public class EpisodeQuizEngineDTO
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("episodeId")]
        public string EpisodeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("expirationDate")]
        public long ExpirationDate { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}