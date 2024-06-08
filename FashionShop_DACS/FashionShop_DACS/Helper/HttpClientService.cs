using FashionShop_DACS.Helper.Abstract;

namespace FashionShop_DACS.Helper
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<string> GetDataAsync(string url)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostDataAsync(string url, HttpContent content)
        {
            var client = CreateHttpClient();
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PutDataAsync(string url, HttpContent content)
        {
            var client = CreateHttpClient();
            var response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> DeleteDataAsync(string url)
        {
            var client = CreateHttpClient();
            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
