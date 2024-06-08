using System.Security.Cryptography;

namespace FashionShop_DACS_API
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public class JwtHelper
    {
        public static string GenerateRandomSecretKey()
        {
            const int keySizeInBytes = 32; // Độ dài của khóa 256 bit
            var rng = new RNGCryptoServiceProvider();
            var key = new byte[keySizeInBytes];
            rng.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }
}
