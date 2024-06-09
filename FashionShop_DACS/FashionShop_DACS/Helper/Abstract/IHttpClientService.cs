namespace FashionShop_DACS.Helper.Abstract
{
    public interface IHttpClientService
    {
        Task<string> GetDataAsync(string url, string token = null);
        Task<string> PostDataAsync(string url, HttpContent content, string token = null);
        Task<string> PutDataAsync(string url, HttpContent content, string token = null);
        Task<bool> DeleteDataAsync(string url, string token = null);
    }
}
