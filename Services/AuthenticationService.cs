using NADT.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace NADT.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        //Your Token API endpoint will most likely be the same as this one, but change it if it's not
        private readonly string FetchTokenUri =
            "https://eastus.api.cognitive.microsoft.com/sts/v1.0/issueToken";

        private readonly string _subscriptionKey;

        public AuthenticationService(string csAccountKey)
        {
            _subscriptionKey = csAccountKey;
        }

        public async Task<string> FetchTokenAsync()
        {
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
                var uriBuilder = new UriBuilder(FetchTokenUri);

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}