using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Vlast.Gamific.Web.Services.Engine.DTO
{
    [DataContract]
    public class QuizQuestionEngineDTO
    {
 
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("enunciation")]
        public string Enunciation { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("quizSheetId")]
        public string QuizSheetId { get; set; }

        [JsonProperty("mediaUrl")]
        public string MediaUrl { get; set; }

        [JsonProperty("optionsString")]
        public string OptionsString { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; }

        [JsonProperty("correctOptionIndex")]
        public int CorrectOptionIndex { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("_links")]
        public Link Links { get; set; }

    }
}