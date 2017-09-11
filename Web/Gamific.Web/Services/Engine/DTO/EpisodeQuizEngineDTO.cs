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

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}