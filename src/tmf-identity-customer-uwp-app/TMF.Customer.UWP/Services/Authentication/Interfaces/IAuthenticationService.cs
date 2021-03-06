using System.Threading.Tasks;

namespace TMF.Customer.UWP.Services.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationData> AuthenticateAsync();
        Task SignOut();
    }
}
