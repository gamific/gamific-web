using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    [DataContract]
    public class QuizQuestionEngineDTO
    {
 
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("quizId")]
        public string QuizId { get; set; }

        [JsonProperty("correctAnswer")]
        public string CorrectAnswer { get; set; }

        [JsonProperty("pointsVale")]
        public int? PointsVale { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }
    }
}