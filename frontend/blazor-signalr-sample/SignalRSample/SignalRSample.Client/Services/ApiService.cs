using IdentityModel.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using SignalRSample.Web.Models;

namespace SignalRSample.Web.Services
{

    public class ApiService
    {
        public HttpClient _httpClient;

        public ApiService(HttpClient client)
        {
            _httpClient = client;
        }

        private async Task<string> requestNewToken()
        {
            try
            {
                var discovery = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(
                    _httpClient, "http://localhost:5000");

                if (discovery.IsError)
                {
                    throw new ApplicationException($"Error: {discovery.Error}");
                }

                var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(_httpClient, new ClientCredentialsTokenRequest
                {
                    Scope = "push-api.full_access",
                    ClientSecret = "thisismyclientspecificsecret",
                    Address = discovery.TokenEndpoint,
                    ClientId = "blazorcontacts-web"
                });

                if (tokenResponse.IsError)
                {
                    throw new ApplicationException($"Error: {tokenResponse.Error}");
                }

                return tokenResponse.AccessToken;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }

        public Task<string> GetAccessToken()
        {
            return requestNewToken();
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            var access_token = await requestNewToken();
            _httpClient.SetBearerToken(access_token);

            var response = await _httpClient.GetAsync("http://localhost:5002/api/contacts");
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Contact>>(responseContent);
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            var access_token = await requestNewToken();
            _httpClient.SetBearerToken(access_token);

            var response = await _httpClient.GetAsync($"http://localhost:5002/api/contacts/{id}");
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Contact>(responseContent);
        }
    }
}
