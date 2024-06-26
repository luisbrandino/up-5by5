
using Newtonsoft.Json;

namespace UPBank.Accounts.Api
{
    public class HttpClientConsumer : IConsumer
    {
        public async Task<T?> Get<T>(string url) where T : new()
        {
            try
            {
                using HttpClient client = new();

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                T? data = JsonConvert.DeserializeObject<T>(responseBody);

                return data;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
