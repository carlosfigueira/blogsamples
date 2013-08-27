using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace AzureMobile.AuthExtensions.WPF
{
    public static class AzureMobileAuthExtensions
    {
        public async static Task<MobileServiceUser> LoginAsync(this MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            Uri startUri = new Uri(client.ApplicationUri, "login/" + provider.ToString().ToLowerInvariant());
            Uri endUri = new Uri(client.ApplicationUri, "login/done");
            LoginPage loginPage = new LoginPage(startUri, endUri);
            string token = await loginPage.Display();
            JObject tokenObj = JObject.Parse(token.Replace("%2C", ","));
            var userId = tokenObj["user"]["userId"].ToObject<string>();
            var authToken = tokenObj["authenticationToken"].ToObject<string>();
            var result = new MobileServiceUser(userId);
            result.MobileServiceAuthenticationToken = authToken;
            client.CurrentUser = result;
            return result;
        }
    }
}
