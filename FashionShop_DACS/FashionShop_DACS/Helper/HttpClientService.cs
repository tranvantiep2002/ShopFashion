using FashionShop_DACS.Helper.Abstract;
using NuGet.Common;
using System.Net.Http.Headers;

namespace FashionShop_DACS.Helper
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateHttpClient(string token = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task<string> GetDataAsync(string url, string token)
        {
            var client = CreateHttpClient(token);
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostDataAsync(string url, HttpContent content, string token)
        {
            var client = CreateHttpClient(token);
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PutDataAsync(string url, HttpContent content, string token)
        {
            var client = CreateHttpClient(token);
            var response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> DeleteDataAsync(string url, string token)
        {
            var client = CreateHttpClient(token);
            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
