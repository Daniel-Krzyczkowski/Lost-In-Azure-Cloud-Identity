using System.Threading.Tasks;
using TMF.Corporate.WebApp.Dto;

namespace TMF.Corporate.WebApp.Services
{
    internal interface IApiService
    {
        Task<string> GetGreetingFromApiAsync();
    }
}
