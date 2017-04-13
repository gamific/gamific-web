using Newtonsoft.Json;

namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    public class ItemEngineDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}