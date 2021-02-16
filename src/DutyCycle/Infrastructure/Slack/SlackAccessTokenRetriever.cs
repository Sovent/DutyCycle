using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DutyCycle.Groups.Domain.Organizations;

namespace DutyCycle.Infrastructure.Slack
{
    public class SlackAccessTokenRetriever : ISlackAccessTokenRetriever
    {
        public SlackAccessTokenRetriever(HttpClient httpClient, string clientId, string clientSecret)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
        }

        public async Task<string> RetrieveToken(string authenticationCode)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", authenticationCode),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret)
            });

            try
            {
                var httpResponse = await _httpClient.PostAsync("api/oauth.v2.access", formContent);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new SlackAccessException(
                        $"Access token request failed with status code {httpResponse.StatusCode:G}");
                }
            
                var response = await httpResponse.Content.ReadFromJsonAsync<GetAccessToSlackResponse>();
                if (response.IsSuccess)
                {
                    return response.AccessToken;
                }
                
                throw new SlackAccessException("Access token request failed with error:" + response.Error);
            }
            catch (Exception e)
            {
                throw new SlackAccessException("Access token request failed", e);
            }
        }
        
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
    }
}