using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMF.Customer.UWP.Config;
using TMF.Customer.UWP.Services.Authentication;
using TMF.Customer.UWP.Services.Authentication.Interfaces;

namespace TMF.AzureAD.UWP.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPublicClientApplication _publicClientApp;

        public AuthenticationService()
        {
            _publicClientApp = PublicClientApplicationBuilder.Create(AuthenticationServiceConfiguration.ClientId)
                .WithB2CAuthority(AuthenticationServiceConfiguration.Authority)
                .WithRedirectUri(AuthenticationServiceConfiguration.RedirectUri)
                .Build();
        }

        public async Task<AuthenticationData> AuthenticateAsync()
        {
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await _publicClientApp.GetAccountsAsync();
            try
            {
                IAccount currentUserAccount = GetAccountByPolicy(accounts, AuthenticationServiceConfiguration.PolicySignUpSignIn);
                authResult = await _publicClientApp.AcquireTokenSilent(AuthenticationServiceConfiguration.ApiScopes, currentUserAccount)
                    .ExecuteAsync();
                return new AuthenticationData
                {
                    AccessToken = authResult.AccessToken
                };

            }
            catch (MsalUiRequiredException msalUiRequiredException)
            {
                if (msalUiRequiredException.Message.Equals("No account or login hint was passed to the AcquireTokenSilent call."))
                {
                    authResult = await HandleFirstTimeAuthenticationAsync(accounts);
                    if (authResult != null)
                    {
                        return new AuthenticationData
                        {
                            AccessToken = authResult.AccessToken
                        };
                    }
                }

                else
                {
                    System.Diagnostics.Debug.WriteLine(nameof(MsalUiRequiredException) + msalUiRequiredException.Message);
                }
            }

            return null;
        }

        public async Task SignOut()
        {
            IEnumerable<IAccount> accounts = await _publicClientApp
                                                    .GetAccountsAsync()
                                                    .ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                await _publicClientApp.RemoveAsync(firstAccount).ConfigureAwait(false);
            }
            catch (MsalException ex)
            {
                System.Diagnostics.Debug.WriteLine(nameof(MsalUiRequiredException) + ex.Message);
            }
        }

        private async Task<AuthenticationResult> HandleFirstTimeAuthenticationAsync(IEnumerable<IAccount> accounts)
        {
            try
            {
                AuthenticationResult authResult = await _publicClientApp.AcquireTokenInteractive(AuthenticationServiceConfiguration.ApiScopes)
                     .WithAccount(GetAccountByPolicy(accounts, AuthenticationServiceConfiguration.PolicySignUpSignIn))
                     .WithPrompt(Prompt.SelectAccount)
                     .ExecuteAsync();
                return authResult;
            }
            catch (MsalException msalClientException)
            {
                if (!msalClientException.ErrorCode.Equals("authentication_canceled"))
                {
                    System.Diagnostics.Debug.WriteLine(nameof(MsalClientException) + msalClientException.Message);
                }

                return null;
            }
        }

        private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
        {
            foreach (var account in accounts)
            {
                string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];
                if (userIdentifier.EndsWith(policy.ToLower())) return account;
            }

            return null;
        }
    }
}
