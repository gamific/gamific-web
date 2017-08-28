using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    [DataContract]
    public class QuestionAnswerEngineDTO
    {
 
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}