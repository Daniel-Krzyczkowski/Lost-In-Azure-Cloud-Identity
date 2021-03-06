using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TMF.Customer.UWP.Config;
using TMF.Customer.UWP.Services.Api.Interfaces;
using TMF.Customer.UWP.Services.Authentication;

namespace TMF.Customer.UWP.Services.Api
{
    internal class ApiService : IApiService
    {
        public async Task<ApiResponse> GetGreetingFromApiAsync(AuthenticationData authenticationData)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationData.AccessToken);
            var response = await httpClient.GetAsync($"{ApiServiceConfiguration.ApiUrl}/user");
            if (response.IsSuccessStatusCode)
            {
                var apiResponseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse>(apiResponseJson);
            }

            else
            {
                System.Diagnostics.Debug.WriteLine($"API returned status code: {response.StatusCode}");
                return null;
            }
        }
    }
}
