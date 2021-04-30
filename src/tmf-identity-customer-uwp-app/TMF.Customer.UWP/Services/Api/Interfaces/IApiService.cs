using System.Threading.Tasks;
using TMF.Customer.UWP.Services.Authentication;

namespace TMF.Customer.UWP.Services.Api.Interfaces
{
    internal interface IApiService
    {
        Task<ApiResponse> GetGreetingFromApiAsync(AuthenticationData authenticationData);
    }
}
