using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TMF.Corporate.WebApp.Dto;

namespace TMF.Corporate.WebApp.Services
{
    internal class ApiService : IApiService
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ApiService(ITokenAcquisition tokenAcquisition,
                          IConfiguration configuration,
                          HttpClient httpClient)
        {
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private async Task GetAndAddApiAccessTokenToAuthorizationHeaderAsync()
        {
            string[] scopes = new[] { _configuration["AzureAd:Scope"] };
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string> GetGreetingFromApiAsync()
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync("https://localhost:5003/user");
            if (response.IsSuccessStatusCode)
            {
                var apiResponseJson = await response.Content.ReadAsStringAsync();
                var deserializedResponse = JsonSerializer.Deserialize<ApiResponse>(apiResponseJson);
                return deserializedResponse.GreetingFromApi;
            }

            else
            {
                System.Diagnostics.Debug.WriteLine($"API returned status code: {response.StatusCode}");
                return null;
            }
        }
    }
}
