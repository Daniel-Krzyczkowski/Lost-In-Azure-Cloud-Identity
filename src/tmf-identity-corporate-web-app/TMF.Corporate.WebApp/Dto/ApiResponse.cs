using System.Text.Json.Serialization;

namespace TMF.Corporate.WebApp.Dto
{
    internal class ApiResponse
    {
        [JsonPropertyName("greetingFromApi")]
        public string GreetingFromApi { get; set; }
    }
}
