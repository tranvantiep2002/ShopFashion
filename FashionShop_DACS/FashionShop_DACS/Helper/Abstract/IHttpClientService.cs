namespace FashionShop_DACS.Helper.Abstract
{
    public interface IHttpClientService
    {
        Task<string> GetDataAsync(string url);
        Task<string> PostDataAsync(string url, HttpContent content);
        Task<string> PutDataAsync(string url, HttpContent content);
        Task<bool> DeleteDataAsync(string url);
    }
}
