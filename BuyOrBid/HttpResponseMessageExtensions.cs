using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuyOrBid
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T?> ConvertResponseToObject<T>(this HttpResponseMessage httpResponseMessage) where T : class
        {
            try
            {
                byte[] bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                return JsonSerializer.Deserialize<T>(bytes);
            }
            catch
            {
                return default;
            }
        }
    }
}
