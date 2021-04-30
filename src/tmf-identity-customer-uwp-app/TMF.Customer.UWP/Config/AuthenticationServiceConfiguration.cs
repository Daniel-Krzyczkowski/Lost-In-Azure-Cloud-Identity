namespace TMF.Customer.UWP.Config
{
    internal static class AuthenticationServiceConfiguration
    {
        public static string Tenant = "<<azure-ad-b2c-tenant-name>>";
        public static string ClientId = "";
        public static string PolicySignUpSignIn = "B2C_1A_SigninSignUp";
        public static string BaseAuthority = "https://{tenant}.b2clogin.com/tfp/{tenant}.onmicrosoft.com/{policy}/oauth2/v2.0/authorize";
        public static string Authority = BaseAuthority.Replace("{tenant}", Tenant).Replace("{policy}", PolicySignUpSignIn);
        public static readonly string RedirectUri = $"msal{ClientId}://auth";
        public static string[] ApiScopes = { $"openid offline_access https://{Tenant}.onmicrosoft.com/<<web-api-application-id-registered-in-the-azure-ad-b2c>>/API.Read.Access" };
    }
}
